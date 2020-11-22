using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class AcessoMap : IEntityTypeConfiguration<Acesso>
    {
        public void Configure(EntityTypeBuilder<Acesso> builder)
        {
            builder.HasKey(x => x.IdAcesso);
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.Login).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.Property(x => x.Senha).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.Property(x => x.DataCadastro).IsRequired().HasColumnType("datetime");
        }
    }
}
