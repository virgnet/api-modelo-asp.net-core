using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class PesquisarTemaCommand : Notifiable, ICommand
    {
        public string TextoDaBusca { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
