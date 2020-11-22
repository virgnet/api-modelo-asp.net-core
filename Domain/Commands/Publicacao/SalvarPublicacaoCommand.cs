using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class SalvarPublicacaoCommand : Notifiable, ICommand
    {
        public Guid? IdPublicacao { get; set; }
        public Guid? IdTema { get; set; }
        public Guid IdTipoDeConteudo { get; set; }
        public string Identificador { get; set; }
        public string Titulo { get; set; }
        public string Chamada { get; set; }
        public string Conteudo { get; set; }
        public string Tags { get; set; }
        public DateTime DataPublicacao { get; set; }
        public Byte[] Binario { get; set; }
        public Byte[] ImagemCapa { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
