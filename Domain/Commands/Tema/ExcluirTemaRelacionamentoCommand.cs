using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class ExcluirTemaRelacionamentoCommand : Notifiable, ICommand
    {
        public Guid IdTemaRelacionamento { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
