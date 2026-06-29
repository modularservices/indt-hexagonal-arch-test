using Indt.Teste.PropostaService.Api.Contracts.Requests;
using Indt.Teste.PropostaService.Api.Contracts.Responses;
using Indt.Teste.PropostaService.Application.Models;
using Indt.Teste.PropostaService.Application.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace Indt.Teste.PropostaService.Api.Controllers;

[ApiController]
[Route("api/propostas")]
public class PropostasController : ControllerBase
{
    #region private members

    private readonly ICreatePropostaUseCase _createUseCase;
    private readonly IGetPropostaByIdUseCase _getByIdUseCase;
    private readonly IListPropostasUseCase _listPropostasUseCase;
    private readonly IUpdatePropostaStatusUseCase _updateStatusUseCase;

    #endregion

    #region constructors
    public PropostasController(ICreatePropostaUseCase createPropostaUseCase,
                               IGetPropostaByIdUseCase getPropostaByIdUseCase,
                               IListPropostasUseCase listPropostasUseCase,
                               IUpdatePropostaStatusUseCase updatePropostaStatusUseCase)
    {
        _createUseCase = createPropostaUseCase;
        _getByIdUseCase = getPropostaByIdUseCase;
        _listPropostasUseCase = listPropostasUseCase;
        _updateStatusUseCase = updatePropostaStatusUseCase;
    }

    #endregion

    [HttpPost]
    [ProducesResponseType(typeof(CreatePropostaResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreatePropostaResponse>> Create([FromBody] CreatePropostaRequest propostaRequest)
    {
        var result = await _createUseCase.ExecuteAsync(propostaRequest.ClienteId,
                                          propostaRequest.SeguradoraId,
                                          propostaRequest.CorretorId,
                                          propostaRequest.ProdutoSeguroId,
                                          propostaRequest.Valor);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Value!.Id },
            result.Value);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PropostaDetailsReadModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<PropostaDetailsReadModel>>> GetAll()
    {
        var propostas = await _listPropostasUseCase.ExecuteAsync();

        return Ok(propostas);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(PropostaDetailsReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PropostaDetailsReadModel>> GetById([FromRoute] Guid id)
    {
        var result = await _getByIdUseCase.ExecuteAsync(id);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdatePropostaStatusRequest request)
    {
        var result = await _updateStatusUseCase.ExecuteAsync(id, request.Status);

        if (result.IsFailure)
        {
            if (result.Error == "Proposta não encontrada.")
                return NotFound(result.Error);

            return BadRequest(result.Error);
        }

        return NoContent();
    }
}