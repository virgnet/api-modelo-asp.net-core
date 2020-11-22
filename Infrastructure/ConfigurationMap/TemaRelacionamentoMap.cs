using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class TemaRelacionamentoMap : IEntityTypeConfiguration<TemaRelacionamento>
    {
        public void Configure(EntityTypeBuilder<TemaRelacionamento> builder)
        {
            builder.HasKey(x => x.IdTemaRelacionamento);
            builder.Property(x => x.IdTema).IsRequired();
            builder.Property(x => x.IdArea).IsRequired(false);
            builder.Property(x => x.IdDocumento).IsRequired(false);
            builder.Property(x => x.IdProjeto).IsRequired(false);
            builder.Property(x => x.IdSistema).IsRequired(false);

            builder.HasOne(x => x.Tema).WithMany(x => x.Relacionamentos).HasForeignKey(c => c.IdTema);
        }
    }
}
