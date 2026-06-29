namespace Indt.Teste.PropostaService.Application.Ports.Out;

public interface IMessageBus
{
    Task PublishAsync<T>(string queue, T message);
}