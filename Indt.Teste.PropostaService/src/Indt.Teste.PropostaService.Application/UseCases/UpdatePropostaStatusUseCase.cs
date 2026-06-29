using Indt.Teste.PropostaService.Application.Common;
using Indt.Teste.PropostaService.Application.Events;
using Indt.Teste.PropostaService.Application.Ports.In;
using Indt.Teste.PropostaService.Application.Ports.Out;
using Indt.Teste.PropostaService.Domain.Enums;

namespace Indt.Teste.PropostaService.Application.UseCases;

public class UpdatePropostaStatusUseCase : IUpdatePropostaStatusUseCase
{
    private readonly IPropostaRepository _repository;
    private readonly IMessageBus _messageBus;

    public UpdatePropostaStatusUseCase(IPropostaRepository repository, IMessageBus messageBus)
    {
        _repository = repository;
        _messageBus = messageBus;
    }

    public async Task<Result> ExecuteAsync(Guid propostaId, StatusProposta novoStatus)
    {
        var proposta = await _repository.GetByIdAsync(propostaId);

        if (proposta is null)
            return Result.Failure("Proposta não encontrada.");

        if (!Enum.IsDefined(novoStatus))
            return Result.Failure("Status inválido.");

        if (novoStatus == StatusProposta.EmAnalise)
        {
            return Result.Failure("Não é possível alterar o status para 'Em Análise'.");
        }

        if (novoStatus == StatusProposta.Aprovada)
            proposta.Aprovar();

        if (novoStatus == StatusProposta.Rejeitada)
            proposta.Rejeitar();

        await _repository.UpdateAsync(proposta);

        if (novoStatus == StatusProposta.Aprovada)
        {
            await _messageBus.PublishAsync("propostas-aprovadas",
                                            new PropostaAprovadaEvent
                                            {
                                                PropostaId = proposta.Id
                                            });
        }

        return Result.Success();
    }
}