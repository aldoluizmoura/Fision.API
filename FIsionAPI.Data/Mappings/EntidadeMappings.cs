using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIsionAPI.Data.Mappings;

public class EntidadeMappings : IEntityTypeConfiguration<Entidade>
{
    public void Configure(EntityTypeBuilder<Entidade> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasOne(e => e.Pessoa).WithMany(p => p.Entidades);
        builder.HasOne(e => e.Contrato).WithOne(c => c.Entidade).OnDelete(DeleteBehavior.SetNull);
        builder.HasMany(e => e.Especialidades).WithMany(es => es.Entidades);
    }
}
