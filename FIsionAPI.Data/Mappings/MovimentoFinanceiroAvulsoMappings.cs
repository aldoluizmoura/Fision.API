using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FIsionAPI.Data.Mappings
{
    public class MovimentoFinanceiroAvulsoMappings : IEntityTypeConfiguration<MovimentoFinanceiroAvulso>
    {
        public void Configure(EntityTypeBuilder<MovimentoFinanceiroAvulso> builder)
        {
            builder.HasKey(m => m.Id);            
            builder.ToTable("MovimentoFinanceiroAvulso");

        }
    }
}
