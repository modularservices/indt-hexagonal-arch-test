using FluentAssertions;
using Indt.Teste.PropostaService.Domain.Entities;
using Indt.Teste.PropostaService.Domain.Enums;

namespace Indt.Teste.PropostaService.UnitTests.Domain.Entities;

public class PropostaTests
{
    private const string NumeroProposta = "PROP-2024-001";
    private static readonly Guid ClienteId = Guid.NewGuid();
    private static readonly Guid SeguradoraId = Guid.NewGuid();
    private static readonly Guid CorretorId = Guid.NewGuid();
    private static readonly Guid ProdutoSeguroId = Guid.NewGuid();
    private const decimal Valor = 1000m;

    [Fact]
    public void Construtor_ComDadosValidos_DeveInicializarProposta()
    {
        // Arrange
        var antesDaCriacao = DateTime.UtcNow;

        // Act
        var proposta = new Proposta(
            NumeroProposta,
            ClienteId,
            SeguradoraId,
            CorretorId,
            ProdutoSeguroId,
            Valor
        );

        // Assert
        proposta.Id.Should().NotBe(Guid.Empty);
        proposta.NumeroProposta.Should().Be(NumeroProposta);
        proposta.ClienteId.Should().Be(ClienteId);
        proposta.SeguradoraId.Should().Be(SeguradoraId);
        proposta.CorretorId.Should().Be(CorretorId);
        proposta.ProdutoSeguroId.Should().Be(ProdutoSeguroId);
        proposta.Valor.Should().Be(Valor);
        proposta.Status.Should().Be(StatusProposta.EmAnalise);

        proposta.DataCriacao.Should().BeOnOrAfter(antesDaCriacao);
        proposta.DataCriacao.Should().BeOnOrBefore(DateTime.UtcNow);
        proposta.DataCriacao.Kind.Should().Be(DateTimeKind.Utc);
    }
}
