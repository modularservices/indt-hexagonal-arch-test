using Indt.Teste.PropostaService.Application.Ports.Out;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Indt.Teste.PropostaService.Infra.Messaging;

public class RabbitMqMessageBus : IMessageBus
{
    private readonly ConnectionFactory _factory;

    public RabbitMqMessageBus(IOptions<RabbitMqSettings> options)
    {
        var settings = options.Value;

        _factory = new ConnectionFactory
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password
        };
    }

    public async Task PublishAsync<T>(string queue, T message)
    {
        await using var connection = await _factory.CreateConnectionAsync();

        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: queue,
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange: string.Empty,
                                        routingKey: queue,
                                        body: body);
    }
}