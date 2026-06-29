using Dapper;
using Indt.Teste.ContratacaoService.Application.Models;
using Indt.Teste.ContratacaoService.Application.Ports.Out;
using Indt.Teste.ContratacaoService.Domain.Entities;
using Indt.Teste.ContratacaoService.Infra.Persistence.Dapper;

namespace Indt.Teste.ContratacaoService.Infra.Persistence;

public class ContratacaoRepository : IContratacaoRepository
{
    private readonly ISqlConnectionFactory _connectionFactory;

    private const string BaseQuery =
                        """
                        SELECT
                            Id,
                            PropostaId,
                            DataContratacao
                        FROM Contratacao
                        """;

    public ContratacaoRepository(ISqlConnectionFactory connectionFactory) =>
        _connectionFactory = connectionFactory;

    public async Task<List<ContratacaoDetailsReadModel>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        var contratacoes = await connection.QueryAsync<ContratacaoDetailsReadModel>(BaseQuery);

        return [.. contratacoes];
    }

    public async Task<ContratacaoDetailsReadModel?> GetByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        var sql = $"{BaseQuery} WHERE Id = @Id";

        return await connection
                    .QueryFirstOrDefaultAsync<ContratacaoDetailsReadModel>(sql,
                                                                           new { Id = id });
    }

    public async Task SaveAsync(Contratacao contratacao)
    {
        using var connection = _connectionFactory.CreateConnection();

        const string sql =
                    """
                    INSERT INTO Contratacao
                    (
                        Id,
                        PropostaId,
                        DataContratacao
                    )
                    VALUES
                    (
                        @Id,
                        @PropostaId,
                        @DataContratacao
                    )
                    """;

        await connection.ExecuteAsync(
            sql,
            new
            {
                contratacao.Id,
                contratacao.PropostaId,
                contratacao.DataContratacao
            });
    }
}