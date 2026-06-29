using Indt.Teste.PropostaService.Application.Common;
using Indt.Teste.PropostaService.Application.Models;

namespace Indt.Teste.PropostaService.Application.Ports.In
{
    public interface ICreatePropostaUseCase
    {
        Task<Result<CreatePropostaResult>> ExecuteAsync(Guid clienteId,
                                Guid seguradoraId,
                                Guid corretorId,
                                Guid produtoSeguroId,
                                decimal valor);
    }
}
