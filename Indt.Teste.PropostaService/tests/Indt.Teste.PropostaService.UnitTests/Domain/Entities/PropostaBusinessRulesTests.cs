using FluentAssertions;
using Indt.Teste.PropostaService.Domain.Enums;
using Indt.Teste.PropostaService.Domain.Exceptions;
using Indt.Teste.PropostaService.UnitTests.TestHelpers;

namespace Indt.Teste.PropostaService.UnitTests.Domain.Entities;

public class PropostaBusinessRulesTests
{
    [Fact]
    public void Aprovar_QuandoStatusEmAnalise_DeveAlterarStatusParaAprovada()
    {
        // Arrange
        var proposta = new PropostaTestBuilder().Build();

        // Act
        proposta.Aprovar();

        // Assert
        proposta.Status.Should().Be(StatusProposta.Aprovada);
    }

    [Fact]
    public void Rejeitar_QuandoStatusEmAnalise_DeveAlterarStatusParaRejeitada()
    {
        // Arrange
        var proposta = new PropostaTestBuilder().Build();

        // Act
        proposta.Rejeitar();

        // Assert
        proposta.Status.Should().Be(StatusProposta.Rejeitada);
    }

    [Fact]
    public void Aprovar_QuandoStatusAprovada_DeveLancarTransicaoStatusException()
    {
        // Arrange
        var proposta = new PropostaTestBuilder().Build();
        proposta.Aprovar();

        // Act
        var action = () => proposta.Aprovar();

        // Assert
        action.Should()
            .Throw<TransicaoStatusException>()
            .WithMessage("*Transição de status inválida*");
    }

    [Fact]
    public void Aprovar_QuandoStatusRejeitada_DeveLancarTransicaoStatusException()
    {
        // Arrange
        var proposta = new PropostaTestBuilder().Build();
        proposta.Rejeitar();

        // Act
        var action = () => proposta.Aprovar();

        // Assert
        action.Should()
            .Throw<TransicaoStatusException>()
            .WithMessage("*Transição de status inválida*");
    }

    [Fact]
    public void Rejeitar_QuandoStatusAprovada_DeveLancarTransicaoStatusException()
    {
        // Arrange
        var proposta = new PropostaTestBuilder().Build();
        proposta.Aprovar();

        // Act
        var action = () => proposta.Rejeitar();

        // Assert
        action.Should()
            .Throw<TransicaoStatusException>()
            .WithMessage("*Transição de status inválida*");
    }

    [Fact]
    public void Rejeitar_QuandoStatusRejeitada_DeveLancarTransicaoStatusException()
    {
        // Arrange
        var proposta = new PropostaTestBuilder().Build();
        proposta.Rejeitar();

        // Act
        var action = () => proposta.Rejeitar();

        // Assert
        action.Should()
            .Throw<TransicaoStatusException>()
            .WithMessage("*Transição de status inválida*");
    }
}
