using Dapper;
using Indt.Teste.PropostaService.Application.Models;
using Indt.Teste.PropostaService.Application.Ports.Out;
using Indt.Teste.PropostaService.Domain.Entities;
using Indt.Teste.PropostaService.Infra.Persistence.Dapper;
using Indt.Teste.PropostaService.Infra.Persistence.EF;

namespace Indt.Teste.PropostaService.Infra.Persistence;

public class PropostaRepository : IPropostaRepository
{
    private readonly PropostaDbContext _context;
    private readonly ISqlConnectionFactory _connectionFactory;

    private readonly string _baseQuery = """
        SELECT
            p.Id,
            p.NumeroProposta,
            c.Nome AS ClienteNome,
            s.Nome AS SeguradoraNome,
            co.Nome AS CorretorNome,
            ps.Nome AS ProdutoSeguroNome,
            Valor,
            CASE p.Status
                WHEN 1 THEN 'Em Análise'
                WHEN 2 THEN 'Aprovada'
                WHEN 3 THEN 'Rejeitada'
            END AS Status,
            DataCriacao

        FROM Proposta p
        INNER JOIN Cliente c ON c.Id = p.ClienteId
        INNER JOIN Seguradora s ON s.Id = p.SeguradoraId
        INNER JOIN Corretor co ON co.Id = p.CorretorId
        INNER JOIN ProdutoSeguro ps ON ps.Id = p.ProdutoSeguroId
        """;

    public PropostaRepository(PropostaDbContext context, ISqlConnectionFactory connectionFactory)
    {
        _context = context;
        _connectionFactory = connectionFactory;
    }

    public async Task AddAsync(Proposta proposta)
    {
        await _context.Propostas.AddAsync(proposta);
        await _context.SaveChangesAsync();
    }

    public async Task<List<PropostaDetailsReadModel>> GetAllAsync()
    {
        using var connection = _connectionFactory.CreateConnection();

        var propostas = await connection.QueryAsync<PropostaDetailsReadModel>(_baseQuery);

        return [.. propostas];
    }

    public async Task<PropostaDetailsReadModel?> GetDetailsByIdAsync(Guid id)
    {
        using var connection = _connectionFactory.CreateConnection();

        var query = $"{_baseQuery} WHERE p.Id = @Id";

        var proposta = await connection.QueryFirstOrDefaultAsync<PropostaDetailsReadModel>(query, new { id });

        return proposta;
    }

    public async Task<Proposta?> GetByIdAsync(Guid id) =>
        await _context.Propostas.FindAsync(id);

    public async Task UpdateAsync(Proposta proposta)
    {
        _context.Propostas.Update(proposta);
        await _context.SaveChangesAsync();
    }
}