using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class IndicadorMap : IEntityTypeConfiguration<Indicador>
    {
        public void Configure(EntityTypeBuilder<Indicador> builder)
        {
            builder.HasKey(x => x.IdIndicador);
            builder.Property(x => x.Titulo).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.HasOne(x => x.TipoIndicador).WithMany(x => x.Indicadores).HasForeignKey(c => c.IdTipoIndicador);
        }
    }
}