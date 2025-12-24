using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIsionAPI.Data.Mappings;

public class MovimentoFinanceiroMappings : IEntityTypeConfiguration<MovimentoFinanceiroEntidade>
{
    public void Configure(EntityTypeBuilder<MovimentoFinanceiroEntidade> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasOne(m => m.Contrato).WithMany(c => c.Movimentos);
        builder.ToTable("MovimentoFinanceiroEntidade");
    }
}
