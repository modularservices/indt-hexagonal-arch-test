using Indt.Teste.ContratacaoService.Application.Common;
using Indt.Teste.ContratacaoService.Application.Exceptions;
using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.In;
using Indt.Teste.ContratacaoService.Application.Ports.Out;

namespace Indt.Teste.ContratacaoService.Application.UseCases;

public class GetContratacaoByIdUseCase : IGetContratacaoByIdUseCase
{
    private readonly IContratacaoRepository _repository;
    private readonly IPropostaServiceClient _propostaServiceClient;

    public GetContratacaoByIdUseCase(IContratacaoRepository repository,
                                     IPropostaServiceClient propostaServiceClient)
    {
        _repository = repository;
        _propostaServiceClient = propostaServiceClient;
    }

    public async Task<Result<ContratacaoDetailsReadModel>> ExecuteAsync(Guid id)
    {
        var contratacao = await _repository.GetByIdAsync(id);

        if (contratacao is null)
            return Result<ContratacaoDetailsReadModel>
                .Failure("Contratação não encontrada.");

        var proposta = await _propostaServiceClient.GetByIdAsync(contratacao.PropostaId) ??
                        throw new PropostaNaoEncontradaException(contratacao.PropostaId);

        contratacao.NumeroProposta = proposta.NumeroProposta;
        contratacao.ValorProposta = proposta.Valor;

        return Result<ContratacaoDetailsReadModel>
            .Success(contratacao);
    }
}