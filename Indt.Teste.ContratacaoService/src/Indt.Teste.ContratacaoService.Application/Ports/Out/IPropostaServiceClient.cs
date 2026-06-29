using Indt.Teste.ContratacaoService.Application.Models;

namespace Indt.Teste.ContratacaoService.Application.Ports.Out
{
    public interface IPropostaServiceClient
    {
        Task<PropostaReadModel?> GetByIdAsync(Guid propostaId);

        Task ContratarPropostaAsync(Guid propostaId);
    }
}
