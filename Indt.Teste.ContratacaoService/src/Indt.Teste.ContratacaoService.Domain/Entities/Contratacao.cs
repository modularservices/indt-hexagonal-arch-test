namespace Indt.Teste.ContratacaoService.Domain.Entities
{
    public class Contratacao
    {
        public Guid Id { get; private set; }
        public Guid PropostaId { get; private set; }
        public DateTime DataContratacao { get; private set; }

        public Contratacao(Guid propostaId)
        {
            Id = Guid.NewGuid();
            PropostaId = propostaId;
            DataContratacao = DateTime.UtcNow;
        }
    }
}
