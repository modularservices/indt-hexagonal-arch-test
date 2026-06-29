namespace Indt.Teste.PropostaService.Api.Contracts.Responses;

public class CreatePropostaResponse
{
    public Guid Id { get; set; }

    public string NumeroProposta { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}