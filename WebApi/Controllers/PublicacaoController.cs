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
    public class PublicacaoController : ControllerBase
    {
        private readonly IPublicacaoRepository _repository;
        private readonly PublicacaoHandler _handler;

        public PublicacaoController(IPublicacaoRepository repository, PublicacaoHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpPost]
        [Route("v1/publicacao/salvar")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult Post([FromBody]SalvarPublicacaoCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/publicacao/pesquisar")]
        public IEnumerable<PublicacaoQueryResult> GetPublicacoes(PesquisarPublicacaoCommand cmd)
        {
            return _repository.Pesquisar(cmd.IdTema, cmd.TextoDaBusca);
        }

        [HttpGet]
        [Route("v1/publicacao/{idDocumento}")]
        public PublicacaoQueryResult BuscarPublicacao(Guid idPublicacao)
        {
            return _repository.Buscar(idPublicacao);
        }
    }
}