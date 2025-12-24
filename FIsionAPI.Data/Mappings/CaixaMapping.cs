using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIsionAPI.Data.Mappings;

public class CaixaMapping : IEntityTypeConfiguration<Caixa>
{
    public void Configure(EntityTypeBuilder<Caixa> builder)
    {
        builder.HasKey(c => c.Id);
        builder.HasMany(c => c.Movimentos).WithOne(m => m.Caixa);
        builder.HasMany(c => c.MovimentosAvulsos).WithOne(m => m.Caixa);
        builder.ToTable("Caixa");
    }
}
