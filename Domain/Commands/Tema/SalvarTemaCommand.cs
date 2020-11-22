using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class SalvarTemaCommand : Notifiable, ICommand
    {
        public Guid? IdTema { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string ImagemBase64 { get; set; }
        public Byte[] ImagemByte { get; set; }
        public string Tags { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
