using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class PesquisarPublicacaoCommand : Notifiable, ICommand
    {
        public Guid? IdTipoDeConteudo { get; set; }
        public Guid? IdTema { get; set; }
        public string TextoDaBusca { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
