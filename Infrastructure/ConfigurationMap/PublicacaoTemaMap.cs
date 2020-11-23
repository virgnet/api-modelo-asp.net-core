using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class PublicacaoTemaMap : IEntityTypeConfiguration<PublicacaoTema>
    {
        public void Configure(EntityTypeBuilder<PublicacaoTema> builder)
        {
            builder.HasKey(x => new { x.IdPublicacao, x.IdTema });
            builder.Property(x => x.IdPublicacao).IsRequired();
            builder.Property(x => x.IdTema).IsRequired();

            builder.HasOne(x => x.Publicacao).WithMany(x => x.PublicacoesTemas).HasForeignKey(c => c.IdPublicacao);
            builder.HasOne(x => x.Tema).WithMany(x => x.PublicacoesTemas).HasForeignKey(c => c.IdTema);
        }
    }
}
