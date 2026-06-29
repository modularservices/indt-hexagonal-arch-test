using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Common;

namespace Indt.Teste.ContratacaoService.Application.Ports.In;

public interface IGetContratacaoByIdUseCase
{
    Task<Result<ContratacaoDetailsReadModel>> ExecuteAsync(Guid id);
}
