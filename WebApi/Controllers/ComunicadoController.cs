using Domain.Commands;
using Domain.Handlers;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace WebApi.Controllers
{
    [ApiController]
    public class ComunicadoController : ControllerBase
    {
        private readonly IComunicadoRepository _repository;
        private readonly ComunicadoHandler _handler;

        public ComunicadoController(IComunicadoRepository repository, ComunicadoHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/comunicado/listar-template")]
        public IEnumerable<TemplateComunicadoQueryResult> ListarTemplate()
        {
            return _repository.ListarTemplateComunicado();
        }

        [HttpPost]
        [Route("v1/comunicado/pesquisa-banner")]
        public IEnumerable<ComunicadoQueryResult> ListarComunicadoBanner(PesquisarComunicadoCommand cmd)
        {
            return _repository.PesquisarComunicadoBanner(cmd.Contexto, cmd.Login);
        }

        [HttpPost]
        [Route("v1/comunicado/aprovar")]
        public ICommandResult AprovarComunicado(AprovarComunicadoCommand cmd)
        {
            var result = (CommandResult)_handler.Handle(cmd);

            return result;
        }

        [HttpPost]
        [Route("v1/comunicado/desativar")]
        public ICommandResult DesativarComunicado(DesativarComunicadoCommand cmd)
        {
            var result = (CommandResult)_handler.Handle(cmd);

            return result;
        }

        [HttpPost]
        [Route("v1/comunicado/salvar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult Post([FromBody]SalvarComunicadoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/comunicado/salvar-log")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult SalvarLog([FromBody]SalvarComunicadoVersaoLogCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/comunicado/pesquisar")]
        public IEnumerable<ComunicadoQueryResult> GetComunicados(PesquisarComunicadoCommand cmd)
        {
            return _repository.PesquisarComunicado(cmd.IdTema, cmd.IdTemplate, cmd.TextoDaBusca, cmd.Contexto, cmd.Login, cmd.Publicados);
        }

        [HttpGet]
        [Route("v1/comunicado/{id}")]
        public ComunicadoQueryResult BuscarComunicado(Guid id)
        {
            return _repository.BuscarComunicado(id);
        }
    }
}