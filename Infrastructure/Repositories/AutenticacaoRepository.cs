using Dapper;
using Domain.Queries;
using Domain.Repositories;
using Infrastructure.DataContext;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class AutenticacaoRepository : IAutenticacaoRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public AutenticacaoRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public AutenticacaoQueryResult BuscarDadosToken(string idToken, string sistema)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"
                                SELECT T.IdToken AS Token
                                      ,T.DataCadastro AS DataUltimoLogon
                                      ,T.IdFuncionario AS IdFuncionario
                                      ,F.Nome AS Nome
                                      ,F.Login AS Login
                                      ,F.CPF AS CPF
                                      ,F.IdUnidade AS IdUnidade
                                      ,U.Nome AS Unidade
	                                  ,F.IdCoordenacao AS IdCoordenacao
                                      ,C.Nome AS Coordenacao
                                  FROM { _context.PrefixGestaoV1 }[TokenAcesso] AS T
                                  INNER JOIN { _context.PrefixGestaoV1 }[Funcionario] AS F ON F.IdFuncinario = T.IdFuncionario 
                                  INNER JOIN { _context.PrefixGestaoV1 }[Unidade] AS U ON U.IdUnidade = F.IdUnidade
                                  LEFT JOIN { _context.PrefixGestaoV1 }[Coordenacao] AS C ON F.IdCoordenacao = C.IdCoordenacao 
                                  WHERE T.Ativo = 1 AND F.Ativo = 1 AND T.IdToken = '{ idToken }'";

                    connection.Open();

                    var token = connection.Query<AutenticacaoQueryResult>(sqlDocumentos).FirstOrDefault();

                    if (token != null)
                    {
                        var sqlPermissao = $@"SELECT FS.IdSistema AS IdSistema, 
                            FS.IdSistemaPerfil AS IdSistemaPerfil, 
                            SPA.IdSistemaAcao AS IdSistemaAcao, 
                            SA.NomeAcao AS NomeAcao, 
                            SA.ReferenciaAcao AS ReferenciaAcao
                            FROM { _context.PrefixGestaoV1 }[FuncionarioSistema] AS FS
                            INNER JOIN { _context.PrefixGestaoV1 }[Sistema] AS S ON FS.IdSistema = S.IdSistema
                            INNER JOIN { _context.PrefixGestaoV1 }[SistemaPerfilAcao] AS SPA ON SPA.IdSistemaPerfil = FS.IdSistemaPerfil
                            INNER JOIN { _context.PrefixGestaoV1 }[SistemaAcao] AS SA ON SA.IdSistemaAcao = SPA.IdSistemaAcao
                            WHERE S.Referencia like '%{sistema}%' AND FS.IdFuncionario = '{token.IdFuncionario}'";

                        token.Permissoes = connection.Query<AutenticacaoPermisaoQueryResult>(sqlPermissao).ToList();
                    }

                    return token;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
