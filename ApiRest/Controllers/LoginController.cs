using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Shared.Commands;
using Domain.Repositories;
using Domain.Queries;

namespace ApiRest.Controllers
{
    public class LoginController : ControllerBase
    {
        private readonly IAutenticacaoRepository _repository;

        public LoginController(IAutenticacaoRepository repository)
        {
            _repository = repository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Produces("application/json")]
        [Route("autenticacao/login")]
        public object Post(
            [FromBody] User usuario,
            [FromServices] SigningConfigurations signingConfigurations,
            [FromServices] TokenConfigurations tokenConfigurations)
        {
            bool credenciaisValidas = false;

            AutenticacaoQueryResult userLogado = new AutenticacaoQueryResult();
            if (usuario != null && ((!String.IsNullOrEmpty(usuario.Login) && !String.IsNullOrEmpty(usuario.Senha)) || !String.IsNullOrEmpty(usuario.Token)))
            {
                userLogado = _repository.Autenticar(usuario.Token);

                if (userLogado != null)
                    credenciaisValidas = true;
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(userLogado.Login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim("unidade", userLogado.IdUnidade.ToString("N")),
                        new Claim("unidade-sistf", userLogado.IdUnidadeSISTF.ToString()),
                        new Claim("unidade-nome", userLogado.Unidade.ToString()),
                        new Claim("nome", userLogado.Nome.ToString()),
                        new Claim("funcionario-gestaos", userLogado.IdFuncionario.ToString("N")),
                        new Claim("coordenacao", userLogado.IdCoordenacao.ToString("N")),
                        new Claim("login", userLogado.Login),
                        new Claim("perfil", userLogado.Perfil),
                        new Claim("cpf", userLogado.CPF.Replace(".","").Replace("-","")),
                    }
                );

                identity.AddClaims(userLogado.Permissoes.Select(role => new Claim("acao", role.Referencia)));

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);
                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = token,
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }
    }
}
