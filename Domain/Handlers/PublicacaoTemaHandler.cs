using Domain.Commands;
using Domain.Commands.PublicacaoTema;
using Domain.Entities;
using Domain.Repositories;
using Shared.Commands;
using Shared.FluentValidator;

namespace Domain.Handlers
{
    public class PublicacaoTemaHandler : Notifiable,
        ICommandHandler<SalvarPublicacaoTemaCommand>,
        ICommandHandler<ExcluirPublicacaoTemaCommand>
    {
        private readonly IPublicacaoTemaRepository _repository;

        public PublicacaoTemaHandler(IPublicacaoTemaRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(SalvarPublicacaoTemaCommand cmd)
        {
            // Criar a entidade
            var obj = new PublicacaoTema();

            // Validar entidades e VOs
            AddNotifications(obj.Notifications);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrija os campos abaixo", Notifications);

            //_repository.SalvarPublicacao(obj);

            // Retornar o resultado para tela
            return new CommandResult(true, "Cadastro realizado com sucesso.", new
            {
                obj.IdPublicacao
            });
        }

        public ICommandResult Handle(ExcluirPublicacaoTemaCommand cmd)
        {
            var obj = new PublicacaoTema();

            // Retornar o resultado para tela
            return new CommandResult(true, "Desativação realizada com sucesso.", new
            {
                obj.IdPublicacao
            });
        }
    }
}
