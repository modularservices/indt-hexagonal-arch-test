using System.Data;

namespace Indt.Teste.ContratacaoService.Infra.Persistence.Dapper;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}