using Domain.Commands;
using Domain.Handlers;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Shared.Commands;
using System;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    public class RamalController : ControllerBase
    {
        private readonly IRamalRepository _repository;

        public RamalController(IRamalRepository ramalRepository)
        {
            _repository = ramalRepository;
        }

        [HttpGet]
        [Route("v1/ramal/listarRamais")]
        public IEnumerable<RamalQueryResult> ListarRamais([FromServices]IDistributedCache cache)
        {
            string valorJSON = cache.GetString("colaborador-ramais");
            if (valorJSON == null)
            {
                var obj = _repository.ListarRamais();

                valorJSON = JsonConvert.SerializeObject(obj);

                DistributedCacheEntryOptions opcoesCache = new DistributedCacheEntryOptions();
                opcoesCache.SetAbsoluteExpiration(TimeSpan.FromDays(1));

                cache.SetString("colaborador-ramais", valorJSON, opcoesCache);
                return obj;
            }
            else
            {
                return JsonConvert.DeserializeObject<IEnumerable<RamalQueryResult>>(valorJSON);
            }

            //return _repository.ListarRamais();
        }

        [HttpGet]
        [Route("v1/ramal/ramalDetalhe/{login}")]
        public RamalQueryResult DetalheRamal(string login)
        {
            return _repository.DetalheRamal(login);
        }


    }
}