﻿using Domain.Commands;
using Domain.Commands.Publicacao;
using Domain.Entities;
using Domain.Repositories;
using Shared.Commands;
using Shared.FluentValidator;

namespace Domain.Handlers
{
    public class PublicacaoHandler : Notifiable,
        ICommandHandler<SalvarPublicacaoCommand>,
        ICommandHandler<DesativarPublicacaoCommand>
    {
        private readonly IPublicacaoRepository _repository;

        public PublicacaoHandler(IPublicacaoRepository repository)
        {
            _repository = repository;
        }

        public ICommandResult Handle(SalvarPublicacaoCommand cmd)
        {
            // Criar a entidade
            var obj = new Publicacao(cmd.IdPublicacao, cmd.Identificador, cmd.Titulo, cmd.Conteudo, cmd.DataPublicacao);

            // Validar entidades e VOs
            AddNotifications(obj.Notifications);

            if (Invalid)
                return new CommandResult(false, "Por favor, corrija os campos abaixo", Notifications);

            _repository.Salvar(obj);

            // Retornar o resultado para tela
            return new CommandResult(true, "Cadastro realizado com sucesso.", new
            {
                obj.IdPublicacao
            });
        }

        public ICommandResult Handle(DesativarPublicacaoCommand command)
        {
            var obj = new Publicacao();
            obj.Desativar();
            _repository.Desativar(obj);
            // Retornar o resultado para tela
            return new CommandResult(true, "Desativação realizada com sucesso.", new
            {
                obj.IdPublicacao
            });
        }
    }
}
