using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class TemaMap : IEntityTypeConfiguration<Tema>
    {
        public void Configure(EntityTypeBuilder<Tema> builder)
        {
            builder.HasKey(x => x.IdTema);
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.Titulo).IsRequired().HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.Tags).IsRequired(false).HasMaxLength(500).HasColumnType("varchar(500)");
            builder.Property(x => x.Imagem).IsRequired(false);
            builder.Property(x => x.Descricao).IsRequired(false).HasColumnType("varchar(MAX)");
        }
    }
}
