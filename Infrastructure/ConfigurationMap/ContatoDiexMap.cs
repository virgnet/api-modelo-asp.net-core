using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.ConfigurationMap
{
    public class ContatoDiexMap : IEntityTypeConfiguration<ContatoDiex>
    {
        public void Configure(EntityTypeBuilder<ContatoDiex> builder)
        {
            builder.HasKey(x => x.IdContatoDiex);
            builder.Property(x => x.IdAssunto).IsRequired();
            builder.Property(x => x.IdSituacao).IsRequired();
            builder.Property(x => x.Ativo).IsRequired();
            builder.Property(x => x.Origem).IsRequired().HasMaxLength(50).HasColumnType("varchar(50)");
            builder.Property(x => x.Area).IsRequired(false).HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.Nome).IsRequired(false).HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.LoginRemetente).IsRequired(false).HasMaxLength(100).HasColumnType("varchar(100)");
            builder.Property(x => x.Mensagem).IsRequired().HasMaxLength(100).HasColumnType("varchar(MAX)");
            builder.Property(x => x.DataMensagem).IsRequired().HasColumnType("datetime");

            builder.Property(x => x.LoginResposta).IsRequired(false).HasColumnType("varchar(100)");
            builder.Property(x => x.Resposta).IsRequired(false).HasMaxLength(500).HasColumnType("varchar(MAX)");
            builder.Property(x => x.DataResposta).IsRequired(false).HasColumnType("datetime");

            builder.HasOne(x => x.Assunto).WithMany(x => x.ContatosDiexAssunto).HasForeignKey(c => c.IdAssunto);
            builder.HasOne(x => x.Situacao).WithMany(x => x.ContatosDiexSituacao).HasForeignKey(c => c.IdSituacao);
        }
    }
}
