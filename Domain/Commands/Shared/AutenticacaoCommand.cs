using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class AutenticacaoCommand : Notifiable, ICommand
    {
        public string login { get; set; }
        public string senha { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}

