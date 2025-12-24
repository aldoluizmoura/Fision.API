using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FIsionAPI.Data.Contexto
{
    public class FisionContext : DbContext
    {
        public FisionContext(DbContextOptions options):base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Entidade> Entidades { get; set; }
        public DbSet<Especialidades> Especialidades { get; set; }
        public DbSet<EnderecoPessoa> Enderecos { get; set; }
        public DbSet<ContratoFinanceiro> Contratos { get; set; }
        public DbSet<MovimentoFinanceiroEntidade> Movimentos { get; set; }
        public DbSet<MovimentoFinanceiroAvulso> MovimentoAvulso { get; set; }
        public DbSet<Caixa> Caixas{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FisionContext).Assembly);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.UtcNow;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataAtualizacao").CurrentValue = DateTime.UtcNow;
                }                

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
