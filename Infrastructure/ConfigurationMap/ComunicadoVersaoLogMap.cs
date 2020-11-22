using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class ComunicadoVersaoLogMap : IEntityTypeConfiguration<ComunicadoVersaoLog>
    {
        public void Configure(EntityTypeBuilder<ComunicadoVersaoLog> builder)
        {
            builder.HasKey(x => x.IdComunicadoVersaoLog);
            builder.Property(x => x.IdComunicadoVersao).IsRequired();
            builder.Property(x => x.Login).IsRequired().HasMaxLength(50).HasColumnType("varchar(100)");
            builder.Property(x => x.DataLeitura).HasColumnType("datetime");

            builder.HasOne(x => x.ComunicadoVersao).WithMany(x => x.Logs).HasForeignKey(c => c.IdComunicadoVersao);
        }
    }
}
