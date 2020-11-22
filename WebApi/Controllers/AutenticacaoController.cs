using Domain.Commands;
using Domain.Handlers;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace WebApi.Controllers
{
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IAutenticacaoRepository _autenticacaoRepository;
        private readonly ComunicadoHandler _handler;

        public AutenticacaoController(IFuncionarioRepository funcionarioRepository, IAutenticacaoRepository autenticacaoRepository, ComunicadoHandler handler)
        {
            _autenticacaoRepository = autenticacaoRepository;
            _funcionarioRepository = funcionarioRepository;
            _handler = handler;
        }

        [HttpGet]
        [Route("v1/autenticacao/{token}/{sistema}")]
        public AutenticacaoQueryResult BuscarDadosToken(string token, string sistema)
        {
            return _autenticacaoRepository.BuscarDadosToken(token, sistema);
        }

        [HttpPost]
        [Route("v1/autenticacao/logar")]
        public UsuarioAutenticacaoQueryResult Logar(AutenticacaoCommand cmd)
        {
            const string LDAP_PATH = "LDAP://sestsenat.org.br";
            const string LDAP_DOMAIN = "sestsenat.org.br";

            using (var context = new PrincipalContext(ContextType.Domain, LDAP_DOMAIN, "srv_gestaoss", "@ldapDes01"))
            {
                if (context.ValidateCredentials(cmd.login, cmd.senha))
                {
                    using (var de = new System.DirectoryServices.DirectoryEntry(LDAP_PATH))
                    using (var ds = new DirectorySearcher(de))
                    {
                        //INICIO 
                        //Se quiser testar outro usuário é só alterar a variavel aqui (PARA PRODUÇÂO DEIXE COMENTADO)
                        //cmd.login = "geilabeck";
                        //FIM
                        var funcionarioGestaoSS = _funcionarioRepository.BuscarFuncionario(cmd.login);
                        if(funcionarioGestaoSS != null)
                            return new UsuarioAutenticacaoQueryResult {
                                Area = funcionarioGestaoSS.Area,
                                Login = cmd.login,
                                Nome = funcionarioGestaoSS.NomeCompleto,
                                Origem = funcionarioGestaoSS.Origem,
                                CodigoCargo = funcionarioGestaoSS.CodigoCargo,
                                Cargo = funcionarioGestaoSS.Cargo,
                                IdUnidade = funcionarioGestaoSS.IdUnidade,
                                Token = funcionarioGestaoSS.Token,
                                Foto = funcionarioGestaoSS.Foto
                            };
                    }
                }
            }

            return null;
        }
    }
}