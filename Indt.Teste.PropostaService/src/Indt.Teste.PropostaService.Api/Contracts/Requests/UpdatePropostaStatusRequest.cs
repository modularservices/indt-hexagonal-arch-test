using Indt.Teste.PropostaService.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Indt.Teste.PropostaService.Api.Contracts.Requests;

public class UpdatePropostaStatusRequest
{
    [Required]
    public StatusProposta Status { get; set; }
}
