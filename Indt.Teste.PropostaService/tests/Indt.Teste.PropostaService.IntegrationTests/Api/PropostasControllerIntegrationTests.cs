using FluentAssertions;
using Indt.Teste.PropostaService.Api.Contracts.Responses;
using Indt.Teste.PropostaService.IntegrationTests.Fixtures;
using System.Net;
using System.Net.Http.Json;

namespace Indt.Teste.PropostaService.IntegrationTests.Api;

public class PropostasControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>,
                                                    IClassFixture<DatabaseFixture>
{
    private readonly HttpClient _client;

    public PropostasControllerIntegrationTests(CustomWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task Create_DeveCriarProposta()
    {
        var request = new
        {
            clienteId = Guid.Parse("9A0F7E5B-1B74-4C52-8F0D-8A5C73A8B5D1"),
            seguradoraId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"),
            corretorId = Guid.Parse("A1B2C3D4-E5F6-47A8-9B0C-123456789ABC"),
            produtoSeguroId = Guid.Parse("F0E1D2C3-B4A5-4678-9012-ABCDEF123456"),
            valor = 1500m
        };

        var response = await _client.PostAsJsonAsync("/api/propostas",
                                                     request,
                                                     cancellationToken: TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var location = response.Headers.Location;

        location.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAll_DeveRetornarLista()
    {
        var response = await _client.GetAsync("/api/propostas", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetById_QuandoNaoExistir_DeveRetornar404()
    {
        var response = await _client.GetAsync($"/api/propostas/{Guid.NewGuid()}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateStatus_DeveAtualizarStatus()
    {
        // cria proposta
        var createRequest = new
        {
            clienteId = Guid.Parse("9A0F7E5B-1B74-4C52-8F0D-8A5C73A8B5D1"),
            seguradoraId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"),
            corretorId = Guid.Parse("A1B2C3D4-E5F6-47A8-9B0C-123456789ABC"),
            produtoSeguroId = Guid.Parse("F0E1D2C3-B4A5-4678-9012-ABCDEF123456"),
            valor = 2000m
        };

        var createResponse = await _client.PostAsJsonAsync("/api/propostas",
                                                           createRequest,
                                                           cancellationToken: TestContext.Current.CancellationToken);

        var created = await createResponse
                            .Content
                            .ReadFromJsonAsync<CreatePropostaResponse>(cancellationToken: TestContext.Current.CancellationToken);

        // atualiza status (aprovada)
        var updateRequest = new
        {
            status = 2
        };

        var patchResponse = await _client.PatchAsJsonAsync($"/api/propostas/{created!.Id}/status",
                                                           updateRequest,
                                                           cancellationToken: TestContext.Current.CancellationToken);

        patchResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ContratarProposta_DeveContratar()
    {
        // cria proposta
        var createRequest = new
        {
            clienteId = Guid.Parse("9A0F7E5B-1B74-4C52-8F0D-8A5C73A8B5D1"),
            seguradoraId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890"),
            corretorId = Guid.Parse("A1B2C3D4-E5F6-47A8-9B0C-123456789ABC"),
            produtoSeguroId = Guid.Parse("F0E1D2C3-B4A5-4678-9012-ABCDEF123456"),
            valor = 3000m
        };

        var createResponse = await _client.PostAsJsonAsync("/api/propostas",
                                                           createRequest,
                                                           cancellationToken: TestContext.Current.CancellationToken);

        var created = await createResponse
                            .Content
                            .ReadFromJsonAsync<CreatePropostaResponse>(cancellationToken: TestContext.Current.CancellationToken);

        // atualiza status (aprovada)
        var updateRequest = new
        {
            status = 2
        };

        await _client.PatchAsJsonAsync($"/api/propostas/{created!.Id}/status",
                                       updateRequest,
                                       cancellationToken: TestContext.Current.CancellationToken);

        // contrata proposta
        var contrataResponse = await _client.PostAsync($"/internal/propostas/{created!.Id}/contratar",
                                                       null,
                                                       TestContext.Current.CancellationToken);

        contrataResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}