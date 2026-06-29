using Indt.Teste.PropostaService.Application.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace Indt.Teste.PropostaService.Api.Controllers;

[ApiController]
[Route("internal/propostas")]
public class InternalController : ControllerBase
{
    private readonly IContractPropostaUseCase _contratarUseCase;

    public InternalController(IContractPropostaUseCase contratarUseCase) =>
        _contratarUseCase = contratarUseCase;

    [HttpPost("{id:guid}/contratar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Contratar([FromRoute] Guid id)
    {
        await _contratarUseCase.ExecuteAsync(id);

        return NoContent();
    }
}
