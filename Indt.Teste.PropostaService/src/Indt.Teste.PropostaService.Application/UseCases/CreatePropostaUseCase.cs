using Indt.Teste.PropostaService.Application.Common;
using Indt.Teste.PropostaService.Application.Models;
using Indt.Teste.PropostaService.Application.Ports.In;
using Indt.Teste.PropostaService.Application.Ports.Out;
using Indt.Teste.PropostaService.Domain.Entities;

namespace Indt.Teste.PropostaService.Application.UseCases;

public class CreatePropostaUseCase : ICreatePropostaUseCase
{
    private readonly IPropostaRepository _repository;

    public CreatePropostaUseCase(IPropostaRepository repository) =>
        _repository = repository;

    public async Task<Result<CreatePropostaResult>> ExecuteAsync(Guid clienteId,
                                                                 Guid seguradoraId,
                                                                 Guid corretorId,
                                                                 Guid produtoSeguroId,
                                                                 decimal valor)
    {
        var proposta = new Proposta(numeroProposta: GerarNumeroProposta(),
                                    clienteId: clienteId,
                                    seguradoraId: seguradoraId,
                                    corretorId: corretorId,
                                    produtoSeguroId: produtoSeguroId,
                                    valor: valor);

        try
        {
            await _repository.AddAsync(proposta);

            var result = new CreatePropostaResult
            {
                Id = proposta.Id,
                NumeroProposta = proposta.NumeroProposta,
                Status = proposta.Status.ToString()
            };

            return Result<CreatePropostaResult>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<CreatePropostaResult>.Failure($"Erro ao criar proposta: {ex.Message} {ex.InnerException}");
        }
    }

    private static string GerarNumeroProposta() =>
            Guid.NewGuid()
            .ToString("N")[..10]
            .ToUpper();
}