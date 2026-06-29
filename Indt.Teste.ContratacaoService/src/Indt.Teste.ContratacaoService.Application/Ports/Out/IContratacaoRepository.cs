using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Domain.Entities;

namespace Indt.Teste.ContratacaoService.Application.Ports.Out;

public interface IContratacaoRepository
{
    Task<List<ContratacaoDetailsReadModel>> GetAllAsync();

    Task<ContratacaoDetailsReadModel?> GetByIdAsync(Guid id);

    Task SaveAsync(Contratacao contratacao);
}