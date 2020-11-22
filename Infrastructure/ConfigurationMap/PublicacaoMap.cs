using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class PublicacaoMap : IEntityTypeConfiguration<Publicacao>
    {
        public void Configure(EntityTypeBuilder<Publicacao> builder)
        {
            builder.HasKey(x => x.IdPublicacao);
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.Titulo).IsRequired().HasMaxLength(200).HasColumnType("varchar(200)");
            builder.Property(x => x.Tags).IsRequired(false).HasMaxLength(500).HasColumnType("varchar(500)");
            builder.Property(x => x.DataCadastro).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.Chamada).IsRequired(false).HasMaxLength(5000).HasColumnType("varchar(5000)");
            builder.Property(x => x.Conteudo).IsRequired(false).HasColumnType("varchar(MAX)");
            builder.Property(x => x.DataPublicacao).IsRequired(false).HasColumnType("datetime");
            builder.Property(x => x.Binario).IsRequired(false);
            builder.Property(x => x.ImagemCapa).IsRequired(false);
            builder.Property(x => x.IdTema).IsRequired(false);
            builder.Property(x => x.IdTipoDeConteudo).IsRequired();
            builder.Property(x => x.Identificador).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");

            builder.HasOne(x => x.Tema).WithMany(x => x.Publicacoes).HasForeignKey(c => c.IdTema);
            builder.HasOne(x => x.TipoDeConteudo).WithMany(x => x.PublicacoesTipoConteudo).HasForeignKey(c => c.IdTipoDeConteudo);
        }
    }
}
