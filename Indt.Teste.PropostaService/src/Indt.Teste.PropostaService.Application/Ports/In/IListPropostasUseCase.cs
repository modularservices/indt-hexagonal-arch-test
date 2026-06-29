using Indt.Teste.PropostaService.Application.Models;

namespace Indt.Teste.PropostaService.Application.Ports.In
{
    public interface IListPropostasUseCase
    {
        Task<List<PropostaDetailsReadModel>> ExecuteAsync();
    }
}