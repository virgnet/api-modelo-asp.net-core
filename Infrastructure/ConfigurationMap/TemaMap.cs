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
        }
    }
}
