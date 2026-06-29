using Dapper;
using Indt.Teste.PropostaService.Api.Setup;
using Microsoft.Data.SqlClient;

namespace Indt.Teste.PropostaService.Api.Extensions;

public static class DatabaseSetupExtensions
{
    public static async Task SetupDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var connectionString = configuration.GetConnectionString("ServerConnection");

        await using var connection = new SqlConnection(connectionString);

        var env = app.Environment.IsDevelopment() ? "Dev" : "";

        var createDbScript = DatabaseScripts.CreateDB.Replace("#Env", env);

        await connection.ExecuteAsync(createDbScript);

        var setupScript = DatabaseScripts.Setup.Replace("#Env", env);

        await connection.ExecuteAsync(setupScript);
    }
}
