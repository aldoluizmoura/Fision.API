using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIsionAPI.Data.Mappings;

public class PessoaMappings : IEntityTypeConfiguration<Pessoa>
{
    public void Configure(EntityTypeBuilder<Pessoa> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.CPF).IsRequired().HasMaxLength(11).HasColumnType("varchar(11)");
        builder.HasOne(p => p.Endereco).WithOne(e => e.Pessoa).OnDelete(DeleteBehavior.SetNull);
        builder.ToTable("Pessoa");
    }
}
