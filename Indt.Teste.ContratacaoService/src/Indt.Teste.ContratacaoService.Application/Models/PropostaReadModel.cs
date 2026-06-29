namespace Indt.Teste.ContratacaoService.Application.Models
{
    public class PropostaReadModel
    {
        public string NumeroProposta { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataCriacao { get; set; }
    }
}
