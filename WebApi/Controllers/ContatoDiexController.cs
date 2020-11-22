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
    public class ContatoDiexController : ControllerBase
    {
        private readonly IContatoDiexRepository _repository;
        private readonly ContatoDiexHandler _handler;

        public ContatoDiexController(IContatoDiexRepository repository, ContatoDiexHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/contato-diex/listar-assunto")]
        public IEnumerable<AssuntoContatoDiexQueryResult> ListarAssunto()
        {
            return _repository.ListarAssuntoContatoDiex();
        }

        [HttpGet]
        [Route("v1/contato-diex/listar-situacao")]
        public IEnumerable<SituacaoContatoDiexQueryResult> ListarSituacao()
        {
            return _repository.ListarSituacaoContatoDiex();
        }

        [HttpPost]
        [Route("v1/contato-diex/salvar-mensagem")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult Post([FromBody]SalvarContatoDiexMensagemCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/contato-diex/salvar-resposta")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public ICommandResult Post([FromBody]SalvarContatoDiexRespostaCommand command)
        {
            var result = (CommandResult)_handler.Handle(command);
            return result;
        }

        [HttpPost]
        [Route("v1/contato-diex/ler")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public bool Ler(Guid idcontatoDiex)
        {
            return _repository.LerContatoDiex(idcontatoDiex);
        }

        [HttpPost]
        [Route("v1/contato-diex/pesquisar")]
        public IEnumerable<ContatoDiexQueryResult> GetContatosDiex(PesquisarContatoDiexCommand cmd)
        {
            return _repository.PesquisarContatoDiex(cmd.IdAssunto, cmd.IdSituacao, cmd.TextoDaBusca);
        }

        [HttpGet]
        [Route("v1/contato-diex/{id}")]
        public ContatoDiexQueryResult BuscarContatoDiex(Guid id)
        {
            return _repository.BuscarContatoDiex(id);
        }

    }
}