using Indt.Teste.PropostaService.Application.Models;
using Indt.Teste.PropostaService.Domain.Entities;

namespace Indt.Teste.PropostaService.Application.Ports.Out;

public interface IPropostaRepository
{
    Task AddAsync(Proposta proposta);

    Task<List<PropostaDetailsReadModel>> GetAllAsync();

    Task<PropostaDetailsReadModel?> GetDetailsByIdAsync(Guid id);

    Task<Proposta?> GetByIdAsync(Guid id);

    Task UpdateAsync(Proposta proposta);
}