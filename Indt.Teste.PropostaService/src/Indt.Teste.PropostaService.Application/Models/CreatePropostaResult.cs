namespace Indt.Teste.PropostaService.Application.Models;

public class CreatePropostaResult
{
    public Guid Id { get; set; }

    public string NumeroProposta { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}