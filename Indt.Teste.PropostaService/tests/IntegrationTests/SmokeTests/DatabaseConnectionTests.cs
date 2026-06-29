using FluentAssertions;
using Indt.Teste.PropostaService.IntegrationTests.Fixtures;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Indt.Teste.PropostaService.IntegrationTests.SmokeTests;

public class DatabaseConnectionTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public DatabaseConnectionTests(DatabaseFixture fixture) =>
        _fixture = fixture;

    [Fact]
    public async Task DeveConectarNoBanco()
    {
        using var connection = new SqlConnection(DatabaseFixture.ConnectionString);

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        connection.State.Should().Be(ConnectionState.Open);
    }

    [Fact]
    public async Task DeveExecutarSelect1()
    {
        using var connection = new SqlConnection(DatabaseFixture.ConnectionString);

        await connection.OpenAsync(TestContext.Current.CancellationToken);

        using var command = new SqlCommand("SELECT 1", connection);

        var result = await command.ExecuteScalarAsync(TestContext.Current.CancellationToken);

        result.Should().Be(1);
    }
}