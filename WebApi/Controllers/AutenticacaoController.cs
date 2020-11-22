using Domain.Commands;
using Domain.Queries;
using Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace WebApi.Controllers
{
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IAutenticacaoRepository _autenticacaoRepository;

        public AutenticacaoController(IAutenticacaoRepository autenticacaoRepository)
        {
            _autenticacaoRepository = autenticacaoRepository;
        }

        [HttpPost]
        [Route("v1/autenticacao/logar")]
        public UsuarioAutenticacaoQueryResult LogarLDAP(AutenticacaoCommand cmd)
        {
            const string LDAP_PATH = "LDAP://xxx.org.br";
            const string LDAP_DOMAIN = "xxx.org.br";

            using (var context = new PrincipalContext(ContextType.Domain, LDAP_DOMAIN, "???", "???"))
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
                    }
                }
            }

            return null;
        }
    }
}