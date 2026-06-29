using System.Data;

namespace Indt.Teste.PropostaService.Infra.Persistence.Dapper;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}