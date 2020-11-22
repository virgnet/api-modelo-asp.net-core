using Shared.FluentValidator;
using System;

namespace Domain.Entities
{
    public class PublicacaoTema : Notifiable
    {
        public Guid IdPublicacao { get; private set; }
        public Publicacao Publicacao { get; set; }
        public Guid IdTema { get; private set; }
        public Tema Tema { get; set; }
    }
}
