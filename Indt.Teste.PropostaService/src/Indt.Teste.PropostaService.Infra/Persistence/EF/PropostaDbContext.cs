using Indt.Teste.PropostaService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Indt.Teste.PropostaService.Infra.Persistence.EF;

public class PropostaDbContext : DbContext
{
    public PropostaDbContext(
        DbContextOptions<PropostaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Proposta> Propostas => Set<Proposta>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Proposta>(entity =>
        {
            entity.ToTable("Proposta");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.NumeroProposta)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Valor)
                .HasPrecision(18, 2);

            entity.Property(x => x.Status)
                .HasConversion<byte>();
        });
    }
}