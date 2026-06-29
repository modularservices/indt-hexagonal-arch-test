using FluentAssertions;
using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.Out;
using Indt.Teste.ContratacaoService.Application.UseCases;
using Moq;

namespace Indt.Teste.ContratacaoService.UnitTests.Application.UseCases;

public class ListContratacoesUseCaseTests
{
    private readonly Mock<IContratacaoRepository> _mockRepository;
    private readonly Mock<IPropostaServiceClient> _mockPropostaServiceClient;
    private readonly ListContratacoesUseCase _useCase;

    public ListContratacoesUseCaseTests()
    {
        _mockRepository = new Mock<IContratacaoRepository>();
        _mockPropostaServiceClient = new Mock<IPropostaServiceClient>();
        _useCase = new ListContratacoesUseCase(_mockRepository.Object, _mockPropostaServiceClient.Object);
    }

    [Fact]
    public async Task ExecuteAsync_WhenNoContratacoesExist_ShouldReturnEmptyList()
    {
        // Arrange
        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ContratacaoDetailsReadModel>());

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        result.Should().BeEmpty();
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockPropostaServiceClient.Verify(p => p.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_WhenContratacoesExist_ShouldReturnContratacoesWithPropostaDetails()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var contratacaoId = Guid.NewGuid();
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
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ContratacaoDetailsReadModel> { contratacao });

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId))
            .ReturnsAsync(proposta);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].NumeroProposta.Should().Be("PROP-001");
        result[0].ValorProposta.Should().Be(1000m);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        _mockPropostaServiceClient.Verify(p => p.GetByIdAsync(propostaId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WhenPropostaNotFound_ShouldSkipContratacao()
    {
        // Arrange
        var propostaId = Guid.NewGuid();
        var contratacaoId = Guid.NewGuid();
        var contratacao = new ContratacaoDetailsReadModel
        {
            Id = contratacaoId,
            PropostaId = propostaId,
            DataContratacao = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ContratacaoDetailsReadModel> { contratacao });

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId))
            .ReturnsAsync((PropostaReadModel?)null);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        result.Should().HaveCount(1);
        result[0].NumeroProposta.Should().BeEmpty();
        result[0].ValorProposta.Should().Be(0);
        _mockPropostaServiceClient.Verify(p => p.GetByIdAsync(propostaId), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_WithMultipleContratacoes_ShouldReturnAllWithPropostaDetails()
    {
        // Arrange
        var propostaId1 = Guid.NewGuid();
        var propostaId2 = Guid.NewGuid();
        var contratacao1 = new ContratacaoDetailsReadModel
        {
            Id = Guid.NewGuid(),
            PropostaId = propostaId1,
            DataContratacao = DateTime.UtcNow
        };
        var contratacao2 = new ContratacaoDetailsReadModel
        {
            Id = Guid.NewGuid(),
            PropostaId = propostaId2,
            DataContratacao = DateTime.UtcNow
        };

        var proposta1 = new PropostaReadModel
        {
            NumeroProposta = "PROP-001",
            Valor = 1000m,
            DataCriacao = DateTime.UtcNow
        };
        var proposta2 = new PropostaReadModel
        {
            NumeroProposta = "PROP-002",
            Valor = 2000m,
            DataCriacao = DateTime.UtcNow
        };

        _mockRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<ContratacaoDetailsReadModel> { contratacao1, contratacao2 });

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId1))
            .ReturnsAsync(proposta1);

        _mockPropostaServiceClient
            .Setup(p => p.GetByIdAsync(propostaId2))
            .ReturnsAsync(proposta2);

        // Act
        var result = await _useCase.ExecuteAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0].NumeroProposta.Should().Be("PROP-001");
        result[0].ValorProposta.Should().Be(1000m);
        result[1].NumeroProposta.Should().Be("PROP-002");
        result[1].ValorProposta.Should().Be(2000m);
    }
}
