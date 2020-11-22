using Domain.Commands;
using Domain.Handlers;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    public class TemaController : ControllerBase
    {
        private readonly ITemaRepository _repository;
        private readonly TemaHandler _handler;

        public TemaController(ITemaRepository repository, TemaHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpPost]
        [Route("v1/tema/salvar-tema")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult SalvarTema([FromBody]SalvarTemaCommand command)
        {
            command.ImagemByte = Convert.FromBase64String(command.ImagemBase64);

            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/tema/salvar-relacionamento")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult SalvarRelacionamento([FromBody]SalvarTemaRelacionamentoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/tema/desativar")]
        public ICommandResult DesativarTema(DesativarTemaCommand cmd)
        {
            var result = (CommandResult)_handler.Handle(cmd);

            return result;
        }

        [HttpPost]
        [Route("v1/tema/excluir-relacionamento")]
        public ICommandResult ExcluirRelacionamento(ExcluirTemaRelacionamentoCommand cmd)
        {
            var result = (CommandResult)_handler.Handle(cmd);

            return result;
        }

        [HttpPost]
        [Route("v1/tema/pesquisar")]
        public IEnumerable<TemaQueryResult> GetTemas(PesquisarTemaCommand cmd)
        {
            return _repository.PesquisarTema(cmd.TextoDaBusca);
        }

        [HttpGet]
        [Route("v1/tema/{id}")]
        public TemaQueryResult BuscarTema(Guid id)
        {
            return _repository.BuscarTema(id);
        }

        [HttpGet]
        [Route("v1/tema/listar")]
        public IEnumerable<TemaQueryResult> BuscarTodosTemas()
        {
            return _repository.PesquisarTodosTemas();
        }
    }
}