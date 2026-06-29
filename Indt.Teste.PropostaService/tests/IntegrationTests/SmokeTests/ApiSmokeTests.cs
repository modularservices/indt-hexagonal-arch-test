using FluentAssertions;
using Indt.Teste.PropostaService.IntegrationTests.Fixtures;
using System.Net;

namespace Indt.Teste.PropostaService.IntegrationTests.Api;

public class ApiSmokeTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiSmokeTests(CustomWebApplicationFactory factory) =>
        _client = factory.CreateClient();

    [Fact]
    public async Task GET_HealthCheck_DeveRetornarOk()
    {
        // Act
        var response = await _client.GetAsync("/health", TestContext.Current.CancellationToken);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}