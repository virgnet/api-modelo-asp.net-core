using Domain.Queries;
using System;
using Dapper;
using System.Linq;
using System.Data.SqlClient;

namespace Infrastructure.Query
{
    public class AutenticacaoQuery
    {
        public static AutenticacaoQueryResult Autenticar(string connectionString, string token)
        {
            using (SqlConnection db = new SqlConnection(connectionString))
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
	                        ,SPA.ReferenciaPerfil AS Perfil
                            ,SISTF.IdUnidade AS IdUnidadeSISTF
                        FROM [TokenAcesso] AS T
                        INNER JOIN [Funcionario] AS F ON F.IdFuncinario = T.IdFuncionario 
                        INNER JOIN [FuncionarioSistema] AS S ON S.IdFuncionario = F.IdFuncinario
                        INNER JOIN [Sistema] AS SS ON S.IdSistema = SS.IdSistema
                        INNER JOIN [SistemaPerfil] AS SPA ON SPA.IdSistemaPerfil = S.IdSistemaPerfil
                        INNER JOIN [Unidade] AS U ON U.IdUnidade = F.IdUnidade
                        LEFT JOIN [Coordenacao] AS C ON F.IdCoordenacao = C.IdCoordenacao 
                        LEFT JOIN [CnpjUnidade] AS CNPJ ON CNPJ.IdUnidade = U.IdUnidade AND CNPJ.IdGestao = 'B3E54572-9189-4381-BBBF-84F1209D6525'
                        LEFT JOIN SISTF.dbo.Unidade AS SISTF ON SISTF.CNPJSest = CNPJ.CNPJ
                        WHERE T.Ativo = 1 AND F.Ativo = 1 AND T.IdToken = '{ token }' AND SS.Referencia = 'B3E54572-9189-4381-BBBF-84F1209D6525'";

                    db.Open();

                    var objToken = db.Query<AutenticacaoQueryResult>(sqlDocumentos).FirstOrDefault();

                    if (objToken != null)
                    {
                        var sqlPermissao = $@"SELECT 
                    SA.NomeAcao AS Nome, 
                    SA.ReferenciaAcao AS Referencia
                    FROM [FuncionarioSistema] AS FS
                    INNER JOIN [Sistema] AS S ON FS.IdSistema = S.IdSistema
                    INNER JOIN [SistemaPerfilAcao] AS SPA ON SPA.IdSistemaPerfil = FS.IdSistemaPerfil
                    INNER JOIN [SistemaAcao] AS SA ON SA.IdSistemaAcao = SPA.IdSistemaAcao
                    WHERE S.Referencia = 'B3E54572-9189-4381-BBBF-84F1209D6525' AND FS.IdFuncionario = '{objToken.IdFuncionario}'";

                        objToken.Permissoes = db.Query<PermissaoQueryResult>(sqlPermissao).ToList();
                    }

                    return objToken;
                }
                catch (Exception ex)
                {
                    return null;
                }

            }
        }
    }
}
