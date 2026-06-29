using Indt.Teste.ContratacaoService.Application.Events;
using Indt.Teste.ContratacaoService.Application.Ports.Out;
using Indt.Teste.ContratacaoService.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Indt.Teste.ContratacaoService.Infra.Messaging.Consumers;

public class PropostaAprovadaConsumer : BackgroundService
{
    private readonly RabbitMqSettings _settings;
    private readonly IServiceScopeFactory _scopeFactory;

    public PropostaAprovadaConsumer(IOptions<RabbitMqSettings> options, IServiceScopeFactory scopeFactory)
    {
        _settings = options.Value;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            Port = _settings.Port,
            UserName = _settings.UserName,
            Password = _settings.Password
        };

        var connection = await factory.CreateConnectionAsync(stoppingToken);

        var channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await channel.QueueDeclareAsync(queue: "propostas-aprovadas",
                                        durable: true,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null,
                                        cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            var evento = JsonSerializer.Deserialize<PropostaAprovadaEvent>(json);

            if (evento is not null)
            {
                Console.WriteLine($"Proposta recebida: {evento.PropostaId}");

                var contratacao = new Contratacao(evento.PropostaId);

                using var scope = _scopeFactory.CreateScope();

                var repository = scope
                                .ServiceProvider
                                .GetRequiredService<IContratacaoRepository>();

                var propostaServiceClient = scope
                                            .ServiceProvider
                                            .GetRequiredService<IPropostaServiceClient>();

                try
                {
                    await propostaServiceClient.ContratarPropostaAsync(evento.PropostaId);
                    await repository.SaveAsync(contratacao);

                    await channel.BasicAckAsync(eventArgs.DeliveryTag,
                                                false);
                }
                catch
                {
                    if (channel.IsOpen)
                        await channel.BasicNackAsync(eventArgs.DeliveryTag,
                                                     false,
                                                     true);
                }
            }
        };

        await channel.BasicConsumeAsync(queue: "propostas-aprovadas",
                                        autoAck: false,
                                        consumer: consumer,
                                        cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}