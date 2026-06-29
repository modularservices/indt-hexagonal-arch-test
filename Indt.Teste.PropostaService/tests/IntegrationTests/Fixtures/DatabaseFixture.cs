using Microsoft.Data.SqlClient;

namespace Indt.Teste.PropostaService.IntegrationTests.Fixtures;

public class DatabaseFixture : IAsyncLifetime
{
    public const string ConnectionString =
        "Server=192.168.0.196;" +
        "User Id=sa;" +
        "Password=$olraC84;" +
        "Encrypt=False;" +
        "TrustServerCertificate=True;";

    private static async Task ExecuteScriptAsync()
    {
        await using var connection = new SqlConnection(ConnectionString);

        await connection.OpenAsync();

        await using var command = new SqlCommand(DatabaseScripts.CreateDB, connection);

        await command.ExecuteNonQueryAsync();

        await using var command2 = new SqlCommand(DatabaseScripts.Setup, connection);

        await command2.ExecuteNonQueryAsync();
    }

    public async ValueTask InitializeAsync() =>
        await ExecuteScriptAsync();

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }
}