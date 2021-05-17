using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIsionAPI.Data.Mappings
{
    public class EnderecoPessoaMapping : IEntityTypeConfiguration<EnderecoPessoa>
    {
        public void Configure(EntityTypeBuilder<EnderecoPessoa> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Logradouro).IsRequired().HasColumnType("varchar(150)");
            builder.Property(e => e.Numero).IsRequired().HasColumnType("varchar(50)");
            builder.Property(e => e.Cidade).IsRequired().HasColumnType("varchar(100)");
            builder.Property(e => e.Estado).IsRequired().HasColumnType("varchar(50)");
            builder.Property(e => e.CEP).IsRequired().HasColumnType("varchar(8)");
            builder.Property(e => e.Complemento).HasColumnType("varchar(100)");

            builder.ToTable("EnderecoPessoa");


        }
    }
}
