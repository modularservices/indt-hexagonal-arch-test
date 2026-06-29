using Indt.Teste.PropostaService.Application.Common;
using Indt.Teste.PropostaService.Application.Models;

namespace Indt.Teste.PropostaService.Application.Ports.In;

public interface IGetPropostaByIdUseCase
{
    Task<Result<PropostaDetailsReadModel>> ExecuteAsync(Guid id);
}
