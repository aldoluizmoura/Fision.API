using FIsionAPI.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace FIsionAPI.Data.Mappings
{
    public class ContratoFinanceiroMappings : IEntityTypeConfiguration<ContratoFinanceiro>
    {
        public void Configure(EntityTypeBuilder<ContratoFinanceiro> builder)
        {
            builder.HasKey(cf => cf.Id);            
            builder.ToTable("ContratoFinanceiro");
        }
    }
}
;