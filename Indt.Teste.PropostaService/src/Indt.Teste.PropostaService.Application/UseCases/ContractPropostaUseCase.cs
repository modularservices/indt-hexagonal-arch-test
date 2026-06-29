using Indt.Teste.PropostaService.Application.Ports.In;
using Indt.Teste.PropostaService.Application.Ports.Out;

namespace Indt.Teste.PropostaService.Application.UseCases;

public class ContractPropostaUseCase : IContractPropostaUseCase
{
    private readonly IPropostaRepository
        _repository;

    public ContractPropostaUseCase(IPropostaRepository repository) =>
        _repository = repository;

    public async Task ExecuteAsync(Guid propostaId)
    {
        var proposta = await _repository.GetByIdAsync(propostaId) ??
                        throw new Exception($"Proposta {propostaId} não encontrada.");

        proposta.Contratar();

        await _repository.UpdateAsync(proposta);
    }
}