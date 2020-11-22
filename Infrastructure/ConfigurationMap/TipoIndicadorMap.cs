using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class TipoIndicadorMap : IEntityTypeConfiguration<TipoIndicador>
    {
        public void Configure(EntityTypeBuilder<TipoIndicador> builder)
        {
            builder.HasKey(x => x.IdTipoIndicador);
            builder.Property(x => x.Sigla).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.Property(x => x.Titulo).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
        }
    }
}