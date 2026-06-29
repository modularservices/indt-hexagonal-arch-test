namespace Indt.Teste.PropostaService.Api.Contracts.Requests
{
    public class CreatePropostaRequest
    {
        public Guid ClienteId { get; set; }

        public Guid SeguradoraId { get; set; }

        public Guid CorretorId { get; set; }

        public Guid ProdutoSeguroId { get; set; }

        public decimal Valor { get; set; }
    }
}
