using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Repositories.ConfigurationMap
{
    public class CalendarioMap : IEntityTypeConfiguration<Calendario>
    {
        public void Configure(EntityTypeBuilder<Calendario> builder)
        {
            builder.HasKey(x => x.IdCalendario);
            builder.Property(x => x.IdTema).IsRequired(false);
            builder.Property(x => x.Identificador).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.Local).IsRequired(false).HasMaxLength(200).HasColumnType("varchar(200)");
            builder.Property(x => x.Titulo).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
            builder.Property(x => x.Descricao).IsRequired(false).HasMaxLength(5000).HasColumnType("varchar(5000)");
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.DataInicio).HasColumnType("datetime");
            builder.Property(x => x.DataFim).HasColumnType("datetime");
            builder.Property(x => x.DataCadastro).HasColumnType("datetime");

            builder.HasOne(x => x.Tema).WithMany(x => x.Calendarios).HasForeignKey(c => c.IdTema);
        }
    }
}
