using Shared.FluentValidator;
using Shared.FluentValidator.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Tema : Notifiable
    {
        public Guid IdTema { get; private set; }
        public string Titulo { get; private set; }
        public bool Ativo { get; private set; }

        private readonly IList<Publicacao> _publicacoes;
        public IReadOnlyCollection<Publicacao> Publicacoes => _publicacoes.ToArray();

        public Tema()
        {
            _publicacoes = new List<Publicacao>();
        }

        public Tema(Guid? idTema, string titulo, string tags, string descricao, Byte[] imagem)
        {
            _publicacoes = new List<Publicacao>();

            if (idTema == null)
                IdTema = Guid.NewGuid();
            else
                IdTema = idTema.Value;

            Titulo = titulo;
            Ativo = true;

            AddNotifications(new ValidationContract()
                .HasMaxLen(Titulo, 100, "Título", "Limite máximo de 100 caracteres.")
                .IsNotNullOrEmpty(Titulo, "Título", "Campo vazio.")
            );
        }

        public void Desativar()
        {
            Ativo = false;
        }
    }
}
