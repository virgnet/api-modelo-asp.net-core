using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class DesativarTemaCommand : Notifiable, ICommand
    {
        public Guid IdTema { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
