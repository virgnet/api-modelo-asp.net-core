using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands.Publicacao
{
    public class DesativarPublicacaoCommand : Notifiable, ICommand
    {
        public Guid IdPublicacao { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}
