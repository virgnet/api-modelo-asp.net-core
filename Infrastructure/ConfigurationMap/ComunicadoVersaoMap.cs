using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class ComunicadoVersaoMap : IEntityTypeConfiguration<ComunicadoVersao>
    {
        public void Configure(EntityTypeBuilder<ComunicadoVersao> builder)
        {
            builder.HasKey(x => x.IdComunicadoVersao);
            builder.Property(x => x.IdComunicado).IsRequired();
            builder.Property(x => x.IdTemplate).IsRequired();
            builder.Property(x => x.IdTema).IsRequired(false);
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.Aprovado).IsRequired(false);
            builder.Property(x => x.Versao).IsRequired();
            builder.Property(x => x.CriadoPor).IsRequired(false).HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.Assunto).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
            builder.Property(x => x.Conteudo).IsRequired().HasColumnType("varchar(MAX)");
            builder.Property(x => x.DataCadastro).HasColumnType("datetime");

            builder.HasOne(x => x.Comunicado).WithMany(x => x.Versoes).HasForeignKey(c => c.IdComunicado);
            builder.HasOne(x => x.Template).WithMany(x => x.ComunicadosTemplate).HasForeignKey(c => c.IdTemplate);
            builder.HasOne(x => x.Tema).WithMany(x => x.Comunicados).HasForeignKey(c => c.IdTema);
        }
    }
}
