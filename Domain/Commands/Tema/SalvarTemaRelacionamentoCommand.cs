using Shared.Commands;
using Shared.FluentValidator;
using System;

namespace Domain.Commands
{
    public class SalvarTemaRelacionamentoCommand : Notifiable, ICommand
    {
        public Guid IdTema { get; set; }
        public Guid? IdArea { get; set; }
        public Guid? IdProjeto { get; set; }
        public Guid? IdSistema { get; set; }
        public Guid? IdDocumento { get; set; }

        bool ICommand.Valid()
        {
            return Valid;
        }
    }
}