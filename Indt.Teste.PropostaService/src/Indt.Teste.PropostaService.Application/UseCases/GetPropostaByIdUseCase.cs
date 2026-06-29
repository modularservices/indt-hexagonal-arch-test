using Indt.Teste.PropostaService.Application.Common;
using Indt.Teste.PropostaService.Application.Models;
using Indt.Teste.PropostaService.Application.Ports.In;
using Indt.Teste.PropostaService.Application.Ports.Out;

namespace Indt.Teste.PropostaService.Application.UseCases;

public class GetPropostaByIdUseCase : IGetPropostaByIdUseCase
{
    private readonly IPropostaRepository _repository;

    public GetPropostaByIdUseCase(IPropostaRepository repository) =>
        _repository = repository;

    public async Task<Result<PropostaDetailsReadModel>> ExecuteAsync(Guid id)
    {
        var proposta = await _repository.GetDetailsByIdAsync(id);

        if (proposta is null)
        {
            return Result<PropostaDetailsReadModel>
                .Failure("Proposta não encontrada.");
        }

        return Result<PropostaDetailsReadModel>
            .Success(proposta);
    }
}