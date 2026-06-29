namespace Indt.Teste.ContratacaoService.Application.Models;

public class ContratacaoDetailsReadModel
{
    public Guid Id { get; set; }

    public Guid PropostaId { get; set; }

    public DateTime DataContratacao { get; set; }

    public string NumeroProposta { get; set; } = string.Empty;

    public decimal ValorProposta { get; set; }
}