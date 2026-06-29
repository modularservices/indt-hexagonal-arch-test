using Indt.Teste.PropostaService.Domain.Enums;
using Indt.Teste.PropostaService.Domain.Exceptions;

namespace Indt.Teste.PropostaService.Domain.Entities;

public class Proposta : EntityBase
{
    #region props

    public string NumeroProposta { get; private set; } = string.Empty;

    public Guid ClienteId { get; private set; }

    public Guid SeguradoraId { get; private set; }

    public Guid CorretorId { get; private set; }

    public Guid ProdutoSeguroId { get; private set; }

    public decimal Valor { get; private set; }

    public StatusProposta Status { get; private set; }

    public DateTime DataCriacao { get; private set; }

    #endregion

    protected Proposta()
    {
    }

    #region constructor

    public Proposta(string numeroProposta,
                    Guid clienteId,
                    Guid seguradoraId,
                    Guid corretorId,
                    Guid produtoSeguroId,
                    decimal valor)
    {
        Id = Guid.NewGuid();

        NumeroProposta = numeroProposta;

        ClienteId = clienteId;

        SeguradoraId = seguradoraId;

        CorretorId = corretorId;

        ProdutoSeguroId = produtoSeguroId;

        Valor = valor;

        Status = StatusProposta.EmAnalise;

        DataCriacao = DateTime.UtcNow;
    }

    #endregion

    public void Aprovar() =>
        ChangeStatus(StatusProposta.Aprovada);

    public void Rejeitar() =>
        ChangeStatus(StatusProposta.Rejeitada);

    public void Contratar() =>
        ChangeStatus(StatusProposta.Contratada);

    private void ChangeStatus(StatusProposta novoStatus)
    {
        if (novoStatus != StatusProposta.Contratada)
        {
            //Só pode ser aprovada ou rejeitada se estiver em análise
            if (Status != StatusProposta.EmAnalise)
                throw new TransicaoStatusException(Status.ToString(), novoStatus.ToString());
        }
        else
        {
            //Só pode ser contratada se estiver aprovada
            if (Status != StatusProposta.Aprovada)
                throw new TransicaoStatusException(Status.ToString(), novoStatus.ToString());
        }

        Status = novoStatus;
    }
}
