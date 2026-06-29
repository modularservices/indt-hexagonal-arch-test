using FluentAssertions;
using Indt.Teste.ContratacaoService.Application.Exceptions;
using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.Out;
using Indt.Teste.ContratacaoService.Application.UseCases;
using Moq;

namespace Indt.Teste.ContratacaoService.UnitTests.Application.UseCases;

public class GetContratacaoByIdUseCaseTests
{
    private readonly Mock<IContratacaoRepository> _mockRepository;
    private readonly Mock<IPropostaServiceClient> _mockPropostaServiceClient;
    private readonly GetContratacaoByIdUseCase _useCase;

    public GetContratacaoByIdUseCaseTests()
    {
        _mockRepository = new Mock<IContratacaoRepository>();
        _mockPropostaServiceClient = new Mock<IPropostaServiceClient>();
        _useCase = new GetContratacaoByIdUseCase(_mockRepository.Object, _mockPropostaServiceClient.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenContratacaoNotFound_ShouldReturnFailure()
    {
        // Arrange
        var contratacaoId = Guid.NewGuid();
        _mockRepository
            .Setup(r => r.GetByIdAsync(contratacaoId))
            .ReturnsAsync((ContratacaoDetailsReadModel?)null);

        // Act
        var result = await _useCase.ExecuteAsync(contratacaoId);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Contratação não encontrada.");
        result.Value.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync(contratacaoId), Times.Once);
        _mockPropostaServiceClient.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenContratacaoFoundAndPropostaFound_ShouldReturnSuccess()
    {
        // Arrange
        var contratacaoId = Guid.NewGuid();
        var propostaId = Guid.NewGuid();
        var contratacao = new ContratacaoDetailsReadModel
        {
            Id = contratacaoId,
            PropostaId = propostaId,
            DataContratacao = DateTime.UtcNow
        };

        var proposta = new PropostaReadModel
        {
            NumeroProposta = "PROP-001",
            Valor = 1000m,
            DataCriacao = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(contratacaoId))
            .ReturnsAsync(contratacao);

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId))
            .ReturnsAsync(proposta);

        // Act
        var result = await _useCase.ExecuteAsync(contratacaoId);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value!.NumeroProposta.Should().Be("PROP-001");
        result.Value.ValorProposta.Should().Be(1000m);
        _mockRepository.Verify(r => r.GetByIdAsync(contratacaoId), Times.Once);
        _mockPropostaServiceClient.Verify(p => p.GetByIdAsync(propostaId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPropostaNotFound_ShouldThrowPropostaNaoEncontradaException()
    {
        // Arrange
        var contratacaoId = Guid.NewGuid();
        var propostaId = Guid.NewGuid();
        var contratacao = new ContratacaoDetailsReadModel
        {
            Id = contratacaoId,
            PropostaId = propostaId,
            DataContratacao = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(contratacaoId))
            .ReturnsAsync(contratacao);

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId))
            .ReturnsAsync((PropostaReadModel?)null);

        // Act
        var action = async () => await _useCase.ExecuteAsync(contratacaoId);

        // Assert
        await action.Should()
            .ThrowAsync<PropostaNaoEncontradaException>()
            .WithMessage($"*{propostaId}*");
        _mockRepository.Verify(r => r.GetByIdAsync(contratacaoId), Times.Once);
        _mockPropostaServiceClient.Verify(p => p.GetByIdAsync(propostaId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithDifferentIds_ShouldFetchCorrectContratacao()
    {
        // Arrange
        var contratacaoId1 = Guid.NewGuid();
        var contratacaoId2 = Guid.NewGuid();
        var propostaId = Guid.NewGuid();

        var contratacao = new ContratacaoDetailsReadModel
        {
            Id = contratacaoId1,
            PropostaId = propostaId,
            DataContratacao = DateTime.UtcNow
        };

        var proposta = new PropostaReadModel
        {
            NumeroProposta = "PROP-001",
            Valor = 1000m,
            DataCriacao = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetByIdAsync(contratacaoId1))
            .ReturnsAsync(contratacao);

        _mockRepository
            .Setup(r => r.GetByIdAsync(contratacaoId2))
            .ReturnsAsync((ContratacaoDetailsReadModel?)null);

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId))
            .ReturnsAsync(proposta);

        // Act
        var result1 = await _useCase.ExecuteAsync(contratacaoId1);
        var result2 = await _useCase.ExecuteAsync(contratacaoId2);

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsFailure.Should().BeTrue();
        _mockRepository.Verify(r => r.GetByIdAsync(contratacaoId1), Times.Once);
        _mockRepository.Verify(r => r.GetByIdAsync(contratacaoId2), Times.Once);
    }
}
