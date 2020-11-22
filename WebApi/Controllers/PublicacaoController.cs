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
    public class PublicacaoController : ControllerBase
    {
        private readonly IPublicacaoRepository _repository;
        private readonly PublicacaoHandler _handler;

        public PublicacaoController(IPublicacaoRepository repository, PublicacaoHandler handler)
        {
            _repository = repository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/publicacao/tipo-de-conteudo/listar")]
        public IEnumerable<TipoDeConteudoQueryResult> ListarTipoDeConteudo()
        {
            return _repository.ListarTipoDeConteudo();
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
            return _repository.PesquisarPublicacao(cmd.IdTipoDeConteudo, cmd.IdTema, cmd.TextoDaBusca);
        }

        [HttpGet]
        [Route("v1/publicacao/{idDocumento}")]
        public PublicacaoQueryResult BuscarPublicacao(Guid idPublicacao)
        {
            return _repository.BuscarPublicacao(idPublicacao);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("v1/arquivo/upload")]
        [RequestSizeLimit(52_428_800)]
        public ICommandResult Upload(IFormFile arquivo, IFormFile imagem)
        {
            try
            {
                var cmd = new SalvarPublicacaoCommand();
                cmd.IdPublicacao = Guid.Parse(Request.Form["idPublicacao"].ToString());


                if (arquivo != null && arquivo.Length > 0)
                {
                    var reader = new BinaryReader(arquivo.OpenReadStream());
                    cmd.Binario = reader.ReadBytes((int)arquivo.Length);
                }
                else
                {
                    cmd.Binario = null;
                }

                if (imagem != null && imagem.Length > 0)
                {
                    var reader = new BinaryReader(imagem.OpenReadStream());
                    cmd.ImagemCapa = reader.ReadBytes((int)arquivo.Length);
                }
                else
                {
                    cmd.ImagemCapa = null;
                }

                var result = (CommandResult)_handler.HandleArquivos(cmd);

                return result;

            }
            catch (Exception ex)
            {
                return new CommandResult(true, "Cadastro realizado com sucesso.", new { });
            }
        }

    }
}