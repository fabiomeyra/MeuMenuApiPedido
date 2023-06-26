using MeuMenuPedido.Domain.Interfaces.Context;
using MeuMenuPedido.Infra.CrossCutting.AppSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MeuMenuPedido.Infra.Data.Context;

public sealed class MeuMenuPedidoContext : DbContext, IMeuMenuPedidoContext
{
    public MeuMenuPedidoContext(IOptions<AppSettings> appSettingsOptions)
        : base(ObterContxtContextOptions(appSettingsOptions.Value))
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        ChangeTracker.AutoDetectChangesEnabled = false;
        ChangeTracker.LazyLoadingEnabled = false;
    }

    private static DbContextOptions ObterContxtContextOptions(AppSettings configuracoes)
    {
        var conexao = configuracoes.ConnectionString?.MeuMenuDb;
        return new DbContextOptionsBuilder().UseSqlServer(conexao).Options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuMenuPedidoContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("DataCadastro").CurrentValue = DateTime.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataCadastro").IsModified = false;
            }
        }

        foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataAlteracao") != null))
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Property("DataAlteracao").CurrentValue = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}