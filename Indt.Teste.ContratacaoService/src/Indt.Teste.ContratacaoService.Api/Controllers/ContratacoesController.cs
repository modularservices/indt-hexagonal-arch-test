using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.In;
using Microsoft.AspNetCore.Mvc;

namespace Indt.Teste.ContratacaoService.Api.Controllers;

[ApiController]
[Route("api/contratacoes")]
public class ContratacoesController : ControllerBase
{
    #region private members
    private readonly IListContratacoesUseCase _listContratacoesUseCase;
    private readonly IGetContratacaoByIdUseCase _getContratacaoByIdUseCase;
    #endregion

    #region constructor
    public ContratacoesController(IListContratacoesUseCase listContratacoesUseCase,
                                  IGetContratacaoByIdUseCase getContratacaoByIdUseCase)
    {
        _listContratacoesUseCase = listContratacoesUseCase;
        _getContratacaoByIdUseCase = getContratacaoByIdUseCase;
    }
    #endregion

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContratacaoDetailsReadModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ContratacaoDetailsReadModel>>> GetAll()
    {
        var contratacoes = await _listContratacoesUseCase.ExecuteAsync();

        return Ok(contratacoes);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ContratacaoDetailsReadModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ContratacaoDetailsReadModel>> GetById([FromRoute] Guid id)
    {
        var result = await _getContratacaoByIdUseCase.ExecuteAsync(id);

        if (result.IsFailure)
            return NotFound(result.Error);

        return Ok(result.Value);
    }
}