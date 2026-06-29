using Indt.Teste.ContratacaoService.Application.Models;

namespace Indt.Teste.ContratacaoService.Application.Ports.In
{
    public interface IListContratacoesUseCase
    {
        Task<List<ContratacaoDetailsReadModel>> ExecuteAsync();
    }
}