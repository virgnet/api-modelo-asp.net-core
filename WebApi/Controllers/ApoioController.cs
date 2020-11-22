using Domain.Commands;
using Domain.Handlers;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    public class ApoioController : ControllerBase
    {
        private readonly IPublicacaoRepository _publicacaoRepository;
        private readonly ITemaRepository _temaRepository;
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly TemaHandler _handler;

        public ApoioController(IPublicacaoRepository publicacaoRepository, ITemaRepository temaRepository, IFuncionarioRepository funcionarioRepository, TemaHandler handler)
        {
            _publicacaoRepository = publicacaoRepository;
            _temaRepository = temaRepository;
            _funcionarioRepository = funcionarioRepository;
            _handler = handler;
        }

        [HttpPost]
        [Route("v1/apoio/area-pesquisar")]
        public IEnumerable<AreaQueryResult> GetAreas(PesquisaApoioCommand cmd)
        {
            return _temaRepository.PesquisarArea(cmd.TextoDaBusca);
        }

        [HttpPost]
        [Route("v1/apoio/sistema-pesquisar")]
        public IEnumerable<SistemaQueryResult> GetSistemas(PesquisaApoioCommand cmd)
        {
            return _temaRepository.PesquisarSistema(cmd.TextoDaBusca);
        }

        [HttpPost]
        [Route("v1/apoio/documento-pesquisar")]
        public IEnumerable<DocumentoQueryResult> Getdocumentos(PesquisaApoioCommand cmd)
        {
            return _temaRepository.PesquisarDocumento(cmd.TextoDaBusca);
        }

        [HttpPost]
        [Route("v1/apoio/projeto-pesquisar")]
        public IEnumerable<ProjetoQueryResult> GetProjetos(PesquisaApoioCommand cmd)
        {
            return _temaRepository.PesquisarProjeto(cmd.TextoDaBusca);
        }

        [HttpGet]
        [Route("v1/apoio/aniversariantes-pesquisar")]
        public IEnumerable<AniversarioQueryResult> GetAniversariantes([FromServices]IDistributedCache cache)
        {
            string valorJSON = cache.GetString("colaborador-aniversarios");
            if (valorJSON == null || valorJSON.Equals("null"))
            {
                var obj = _funcionarioRepository.BuscarAniversarios();

                valorJSON = JsonConvert.SerializeObject(obj);

                DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(TimeSpan.FromHours(4));

                cache.SetString("colaborador-aniversarios", valorJSON, opcoesCache);
                return obj;
            }
            else
            {
                return JsonConvert.DeserializeObject<IEnumerable<AniversarioQueryResult>>(valorJSON);
            }

            //return _funcionarioRepository.BuscarAniversarios();
        }

        [HttpGet]
        [Route("v1/apoio/sistemas-por-login/{login}")]
        public IEnumerable<SistemaQueryResult> GetSistemasPorLogin(string login)
        {
            return _funcionarioRepository.BuscarSistemaPorLogin(login);
        }

        [HttpGet]
        [Route("v1/apoio/conselhos-buscar/")]
        public IEnumerable<ConselhoQueryResult> GetConselhos(string login)
        {
            return _funcionarioRepository.BuscarConselhos();
        }

        [HttpGet]
        [Route("v1/apoio/tipo-de-conteudo/")]
        public IEnumerable<TipoDeConteudoQueryResult> GetTiposDeConteudo()
        {
            return _publicacaoRepository.ListarTipoDeConteudo();
        }
    }
}