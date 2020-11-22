using Shared.FluentValidator;
using Shared.FluentValidator.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Publicacao : Notifiable
    {
        public Guid IdPublicacao { get; private set; }
        public string Identificador { get; private set; }
        public string Titulo { get; private set; }
        public string Conteudo { get; private set; }        
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataPublicacao { get; private set; }

        readonly IList<PublicacaoTema> _publicacaoTemas;
        public IReadOnlyCollection<PublicacaoTema> PublicacoesTemas => _publicacaoTemas.ToArray();

        public Publicacao()
        {
            _publicacaoTemas = new List<PublicacaoTema>();
        }

        public Publicacao(Guid? idPublicacao, string identificador, string titulo, string conteudo, DateTime dataPublicacao)
        {
            _publicacaoTemas = new List<PublicacaoTema>();

            if (idPublicacao == Guid.Empty || idPublicacao == null)
                IdPublicacao = Guid.NewGuid();
            else
                IdPublicacao = idPublicacao.Value;

            Identificador = identificador;
            Titulo = titulo;
            Conteudo = conteudo;
            Ativo = true;
            DataPublicacao = dataPublicacao;
            DataCadastro = DateTime.Now;

            AddNotifications(new ValidationContract()
                .HasMaxLen(Titulo, 100, "Título", "Limite máximo de 100 caracteres.")
                .IsNotNullOrEmpty(Titulo, "Título", "Campo vazio.")
                .HasMaxLen(Identificador, 100, "Identificador", "Limite máximo de 100 caracteres.")
            );
        }

        public void Desativar()
        {
            Ativo = false;
        }
    } 
}
