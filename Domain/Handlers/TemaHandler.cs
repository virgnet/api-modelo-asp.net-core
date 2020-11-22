using Domain.Commands;
using Domain.Entities;
using Domain.Repositories;
using Shared.Commands;
using Shared.FluentValidator;

namespace Domain.Handlers
{
    public class TemaHandler : Notifiable,
        ICommandHandler<SalvarTemaCommand>,
        ICommandHandler<SalvarTemaRelacionamentoCommand>
    {
        private readonly ITemaRepository _repository;

        public TemaHandler(ITemaRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(SalvarTemaCommand cmd)
        {
            // Criar a entidade
            var obj = new Tema(cmd.IdTema, cmd.Titulo, cmd.Tags, cmd.Descricao, cmd.ImagemByte);

            // Validar entidades e VOs
            AddNotifications(obj.Notifications);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrija os campos abaixo", Notifications);

            _repository.SalvarTema(obj);

            // Retornar o resultado para tela
            return new CommandResult(true, "Cadastro realizado com sucesso.", new
            {
                obj.IdTema
            });
        }

        public ICommandResult Handle(SalvarTemaRelacionamentoCommand cmd)
        {
            // Criar a entidade
            var obj = new TemaRelacionamento(cmd.IdTema, cmd.IdArea, cmd.IdProjeto, cmd.IdSistema, cmd.IdDocumento);

            // Validar entidades e VOs
            AddNotifications(obj.Notifications);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrija os campos abaixo", Notifications);

            _repository.SalvarTemaRelacionamento(obj);

            // Retornar o resultado para tela
            return new CommandResult(true, "Cadastro realizado com sucesso.", new
            {
                obj.IdTemaRelacionamento
            });
        }

        public ICommandResult Handle(DesativarTemaCommand cmd)
        {
            _repository.DesativarTema(cmd.IdTema);

            // Retornar o resultado para tela
            return new CommandResult(true, "Tema desativado com sucesso.", true);
        }

        public ICommandResult Handle(ExcluirTemaRelacionamentoCommand cmd)
        {
            _repository.ExcluirTemaRelacionamento(cmd.IdTemaRelacionamento);

            // Retornar o resultado para tela
            return new CommandResult(true, "Relacionamento excluido com sucesso.", true);
        }
    }
}
