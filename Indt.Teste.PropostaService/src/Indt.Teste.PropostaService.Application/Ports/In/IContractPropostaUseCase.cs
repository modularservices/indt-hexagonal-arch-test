namespace Indt.Teste.PropostaService.Application.Ports.In;

public interface IContractPropostaUseCase
{
    Task ExecuteAsync(Guid propostaId);
}
