using Domain.Commands;
using Domain.Entities;
using Domain.Repositories;
using Shared.Commands;
using Shared.FluentValidator;

namespace Domain.Handlers
{
    public class PublicacaoHandler : Notifiable,
        ICommandHandler<SalvarPublicacaoCommand>
    {
        private readonly IPublicacaoRepository _repository;

        public PublicacaoHandler(IPublicacaoRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(SalvarPublicacaoCommand cmd)
        {
            // Criar a entidade
            var obj = new Publicacao(cmd.IdPublicacao, cmd.IdTipoDeConteudo, cmd.IdTema, cmd.Identificador, cmd.Titulo, cmd.Chamada, cmd.Conteudo, cmd.Tags, cmd.DataPublicacao);

            // Validar entidades e VOs
            AddNotifications(obj.Notifications);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrija os campos abaixo", Notifications);

            _repository.SalvarPublicacao(obj);

            // Retornar o resultado para tela
            return new CommandResult(true, "Cadastro realizado com sucesso.", new
            {
                obj.IdPublicacao
            });
        }

        public ICommandResult HandleArquivos(SalvarPublicacaoCommand cmd)
        {

            var obj = new Publicacao(cmd.IdPublicacao, cmd.Binario, cmd.ImagemCapa);

            _repository.SalvarArquivos(obj);
            // Retornar o resultado para tela
            return new CommandResult(true, "Cadastro realizado com sucesso.", new
            {
                obj.IdPublicacao
            });
        }

    }
}
