using Indt.Teste.PropostaService.Domain.Entities;

namespace Indt.Teste.PropostaService.UnitTests.TestHelpers;

/// <summary>
/// Builder para criar instâncias de Proposta com valores padrão para testes.
/// </summary>
public class PropostaTestBuilder
{
    private string _numeroProposta = "PROP-2024-001";
    private Guid _clienteId = Guid.NewGuid();
    private Guid _seguradoraId = Guid.NewGuid();
    private Guid _corretorId = Guid.NewGuid();
    private Guid _produtoSeguroId = Guid.NewGuid();
    private decimal _valor = 1000m;

    public PropostaTestBuilder ComNumeroProposta(string numero)
    {
        _numeroProposta = numero;
        return this;
    }

    public PropostaTestBuilder ComClienteId(Guid clienteId)
    {
        _clienteId = clienteId;
        return this;
    }

    public PropostaTestBuilder ComSeguradoraId(Guid seguradoraId)
    {
        _seguradoraId = seguradoraId;
        return this;
    }

    public PropostaTestBuilder ComCorretorId(Guid corretorId)
    {
        _corretorId = corretorId;
        return this;
    }

    public PropostaTestBuilder ComProdutoSeguroId(Guid produtoSeguroId)
    {
        _produtoSeguroId = produtoSeguroId;
        return this;
    }

    public PropostaTestBuilder ComValor(decimal valor)
    {
        _valor = valor;
        return this;
    }

    public Proposta Build()
    {
        return new Proposta(
            _numeroProposta,
            _clienteId,
            _seguradoraId,
            _corretorId,
            _produtoSeguroId,
            _valor
        );
    }
}
