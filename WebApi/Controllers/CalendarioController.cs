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
    public class CalendarioController : ControllerBase
    {
        private readonly ICalendarioRepository _repository;
        private readonly CalendarioHandler _handler;

        public CalendarioController(ICalendarioRepository repository, CalendarioHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpPost]
        [Route("v1/calendario/salvar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult SalvarCalendario([FromBody]SalvarCalendarioCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/calendario/pesquisar")]
        public IEnumerable<CalendarioQueryResult> PesquisarCalendario(PesquisarCalendarioCommand cmd)
        {
            return _repository.PesquisarCalendario(cmd.IdTema, cmd.TextoDaBusca, cmd.ano, cmd.mes);
        }

        [HttpGet]
        [Route("v1/calendario/{id}")]
        public CalendarioQueryResult BuscarCalendario(Guid id)
        {
            return _repository.BuscarCalendario(id);
        }

        [HttpGet]
        [Route("v1/calendario/ListarDatas")]
        public IEnumerable<DateTime> ListarDatas()
        {
            return _repository.ListarDatas();
        }

        [HttpGet]
        [Route("v1/calendario")]
        public IEnumerable<CalendarioQueryResult> ListarCalendario()
        {
            return _repository.ListarCalendario();
        }

        [HttpPost]
        [Route("v1/calendario/desativar")]
        public ICommandResult DesativarCalendario(DesativarCalendarioCommand cmd)
        {
            var result = (CommandResult)_handler.Handle(cmd);

            return result;
        }
    }
}