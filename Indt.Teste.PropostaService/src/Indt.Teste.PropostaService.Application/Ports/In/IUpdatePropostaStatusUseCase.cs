using Indt.Teste.PropostaService.Application.Common;
using Indt.Teste.PropostaService.Domain.Enums;

namespace Indt.Teste.PropostaService.Application.Ports.In
{
    public interface IUpdatePropostaStatusUseCase
    {
        Task<Result> ExecuteAsync(Guid propostaId, StatusProposta status);
    }
}
