namespace Indt.Teste.PropostaService.Application.Models;

public class PropostaDetailsReadModel
{
    public Guid Id { get; set; }

    public string NumeroProposta { get; set; } = string.Empty;

    public string ClienteNome { get; set; } = string.Empty;

    public string SeguradoraNome { get; set; } = string.Empty;

    public string CorretorNome { get; set; } = string.Empty;

    public string ProdutoSeguroNome { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public string Status { get; set; } = string.Empty;

    public DateTime DataCriacao { get; set; }
}
