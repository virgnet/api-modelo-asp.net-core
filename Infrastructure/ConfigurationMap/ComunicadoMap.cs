using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class ComunicadoMap : IEntityTypeConfiguration<Comunicado>
    {
        public void Configure(EntityTypeBuilder<Comunicado> builder)
        {
            builder.HasKey(x => x.IdComunicado);
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.DestinatarioDex).IsRequired(false);
            builder.Property(x => x.DestinatarioDexCoordenador).IsRequired(false);
            builder.Property(x => x.DestinatarioUnidade).IsRequired(false);
            builder.Property(x => x.DestinatarioUnidadeDiretor).IsRequired(false);
            builder.Property(x => x.DestinatarioUnidadeSupervisor).IsRequired(false);
            builder.Property(x => x.DataCadastro).HasColumnType("datetime");
            builder.Property(x => x.DataPublicacao).HasColumnType("datetime");
            builder.Property(x => x.DataAvisoEmail).IsRequired(false).HasColumnType("datetime");
        }
    }
}
