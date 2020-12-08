using Domain.Commands;
using Domain.Handlers;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using System;
using System.Collections.Generic;

namespace ApiRest.Controllers
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
        public ICommandResult Salvar([FromBody]SalvarTemaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/tema/desativar")]
        public ICommandResult Desativar(DesativarTemaCommand cmd)
        {
            var result = (CommandResult)_handler.Handle(cmd);

            return result;
        }

        [HttpPost]
        [Route("v1/tema/pesquisar")]
        public IEnumerable<TemaQueryResult> GetTemas(PesquisarTemaCommand cmd)
        {
            return _repository.Pesquisar(cmd.TextoDaBusca);
        }

        [HttpGet]
        [Route("v1/tema/{id}")]
        public TemaQueryResult BuscarTema(Guid id)
        {
            return _repository.Buscar(id);
        }

        [HttpGet]
        [Route("v1/tema/listar")]
        public IEnumerable<TemaQueryResult> BuscarTodosTemas()
        {
            return _repository.PesquisarTodos();
        }
    }
}