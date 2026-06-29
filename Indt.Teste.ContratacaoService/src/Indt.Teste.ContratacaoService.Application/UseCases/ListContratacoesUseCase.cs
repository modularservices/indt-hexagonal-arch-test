using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.In;
using Indt.Teste.ContratacaoService.Application.Ports.Out;

namespace Indt.Teste.ContratacaoService.Application.UseCases;

public class ListContratacoesUseCase : IListContratacoesUseCase
{
    private readonly IContratacaoRepository _repository;
    private readonly IPropostaServiceClient _propostaServiceClient;

    public ListContratacoesUseCase(IContratacaoRepository repository,
                                   IPropostaServiceClient propostaServiceClient)
    {
        _repository = repository;
        _propostaServiceClient = propostaServiceClient;
    }

    public async Task<List<ContratacaoDetailsReadModel>> ExecuteAsync()
    {
        var contratacoes = await _repository.GetAllAsync();

        var tasks = contratacoes.Select(async contratacao =>
                    {
                        var proposta = await _propostaServiceClient.GetByIdAsync(contratacao.PropostaId);

                        if (proposta is null)
                            return;

                        contratacao.NumeroProposta = proposta.NumeroProposta;
                        contratacao.ValorProposta = proposta.Valor;
                    });

        await Task.WhenAll(tasks);

        return contratacoes;
    }
}