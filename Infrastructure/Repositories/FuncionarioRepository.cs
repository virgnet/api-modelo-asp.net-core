using Dapper;
using Domain.Queries;
using Domain.Repositories;
using Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class FuncionarioRepository : IFuncionarioRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public FuncionarioRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public FuncionarioQueryResult BuscarFuncionario(string login)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlUpdateToken = $@"UPDATE {_context.PrefixGestaoV1}[TokenAcesso] SET Ativo = 0
                    WHERE IdFuncionario = (SELECT [IdFuncinario] FROM {_context.PrefixGestaoV1}[Funcionario] WHERE Login = '{login}') AND Ativo = 1";

                    var sqlInsertToken = $@"INSERT INTO {_context.PrefixGestaoV1}[TokenAcesso] ([IdToken],[Ativo],[DataCadastro],[IdFuncionario])
                    SELECT NEWID(), 1, GETDATE(), [IdFuncinario]
                    FROM {_context.PrefixGestaoV1}[Funcionario]
                    WHERE Login = '{login}'";

                    var sql = $@"SELECT 
                            F.Nome AS NomeCompleto, 
                            ISNULL(U.Nome,'') AS Entidade,
                            ISNULL(C.Nome,'') AS Area,
                            (CASE 
                                WHEN CA.CodigoFuncaoProtheus IS NULL THEN 'unidade'
                                WHEN F.IdUnidade = '35B0DDFC-75AF-4701-8185-DB1F3214D877' AND CA.CodigoFuncaoProtheus IN ('02254','02056','02220','02205','02029','02179','02261','02153','02032','02239','02221','02268','02258','02080','02028','02249','02220','02006','02152','02010','02031','02207') THEN 'dex-coordenador'
                                WHEN F.IdUnidade = '35B0DDFC-75AF-4701-8185-DB1F3214D877' AND CA.CodigoFuncaoProtheus IN ('02035','02219') THEN 'diretor-dex'
                                WHEN F.IdUnidade <> '35B0DDFC-75AF-4701-8185-DB1F3214D877' AND CA.CodigoFuncaoProtheus IN ('02034','02040') THEN 'unidade-diretor'
                                WHEN F.IdUnidade <> '35B0DDFC-75AF-4701-8185-DB1F3214D877' AND CA.CodigoFuncaoProtheus IN ('02078') THEN 'unidade-caf'
                                WHEN F.IdUnidade <> '35B0DDFC-75AF-4701-8185-DB1F3214D877' AND CA.CodigoFuncaoProtheus IN ('02032') THEN 'unidade-coordenador-sest'
                                WHEN F.IdUnidade <> '35B0DDFC-75AF-4701-8185-DB1F3214D877' AND CA.CodigoFuncaoProtheus IN ('02028') THEN 'unidade-coordenador-senat'
                                WHEN CA.CodigoFuncaoProtheus IN ('02251','02252','02237','02226','02251','02233','02267','02236','02227','02234','02228','02235','02250','02238','02253','02241','02255') THEN 'unidade-supervisor'
                                ELSE (CASE WHEN F.IdUnidade = '35B0DDFC-75AF-4701-8185-DB1F3214D877' THEN 'dex' ELSE 'unidade' END)
                            END) AS Origem,
                            F.IdUnidade,
                            CA.CodigoFuncaoProtheus AS CodigoCargo,
                            CA.Nome AS Cargo,
                            T.IdToken AS Token,
                            A.Bytes AS Foto
                            FROM {_context.PrefixGestaoV1}[Funcionario] AS F
                            LEFT JOIN {_context.PrefixGestaoV1}[Cargo] CA ON CA.IdCargo = F.IdCargo
                            LEFT JOIN {_context.PrefixGestaoV1}[Arquivo] A ON A.IdArquivo = F.IdArquivo
                            LEFT JOIN {_context.PrefixGestaoV1}[Unidade] AS U ON U.IdUnidade = F.IdUnidade
                            LEFT JOIN {_context.PrefixGestaoV1}[Coordenacao] AS C ON C.IdCoordenacao = F.IdCoordenacao
                            LEFT JOIN {_context.PrefixGestaoV1}[TokenAcesso] AS T ON F.IdFuncinario = T.IdFuncionario AND T.Ativo = 1
                            WHERE F.Login = '{login}'";

                    connection.Open();
                    connection.Execute(sqlUpdateToken);
                    connection.Execute(sqlInsertToken);

                    return connection.Query<FuncionarioQueryResult>(sql).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<AniversarioQueryResult> BuscarAniversarios()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"DECLARE @hoje datetime = getdate();
                            if datename(dw,getdate()) = 'Friday'
                                begin
                                    with c as (SELECT * FROM [PROTHEUS_PRD].[TMPRD].[dbo].[VW_ANIVERSARIO_PORTAL_COLABORADOR]
		                            WHERE convert(varchar(6),DataNascimento,103)= convert(varchar(6),DATEADD(day, 1, @hoje),103) 
	                                or convert(varchar(6),DataNascimento,103)= convert(varchar(6),@hoje,103) 
	                                or convert(varchar(6),DataNascimento,103)= convert(varchar(6),DATEADD(day, 2, @hoje),103) 
		                            )


                                    select * from c 
		                            group by c.Coordenacao, c.DataNascimento, c.Email, c.Nome
		                            order by day(c.DataNascimento) Desc, Nome
                                end
                            else
                                begin
                                with c as (SELECT * FROM [PROTHEUS_PRD].[TMPRD].[dbo].[VW_ANIVERSARIO_PORTAL_COLABORADOR] WHERE
                                            (Month(CONVERT(DATETIME, DataNascimento, 102)) = month(getdate())
		                                    AND day(CONVERT(DATETIME, DataNascimento, 102)) = day(getdate())))

                                            select * from c 
                                            group by c.Coordenacao, c.DataNascimento, c.Email, c.Nome
                                            order by c.Nome
                                end  ";

                    connection.Open();
                    return connection.Query<AniversarioQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<SistemaQueryResult> BuscarSistemaPorLogin(string login)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"SELECT S.IdSistema
	                                  ,S.Nome as Titulo
	                                  ,S.LinkAcesso as Link
	                                  ,S.Descricao
	                                  ,S.Referencia
	                                  ,S.Icone
	                                  ,S.Style 
	                                  ,ISNULL(SF.NomePerfil, 'Básico') as Perfil
                                FROM {_context.PrefixGestaoV1}[Sistema] S
                                INNER JOIN {_context.PrefixGestaoV1}[FuncionarioSistema] FS ON FS.IdSistema = S.IdSistema
                                LEFT JOIN  {_context.PrefixGestaoV1}[SistemaPerfil] SF ON SF.IdSistemaPerfil = FS.IdSistemaPerfil
                                INNER JOIN {_context.PrefixGestaoV1}[Funcionario] F ON F.IdFuncinario = FS.IdFuncionario
                                WHERE S.Ativo = 1 AND F.Login ='{login}'";
                    
                    connection.Open();
                    return connection.Query<SistemaQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ConselhoQueryResult> BuscarConselhos()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"SELECT C.IdConselho
                                        ,C.Nome as Nome
                                        ,C.Email 
                                        ,C.AreaAtuacao as Atuacao
                                        ,F.Nome as  NomeSupervisor
                                 FROM {_context.PrefixGestaoV1}[Conselho] C
                                 LEFT JOIN {_context.PrefixGestaoV1}[Funcionario] F ON F.IdFuncinario = C.IdSupervisor
                                 WHERE C.Ativo = 1
                                 ORDER BY C.Nome";


                    connection.Open();
                    var conselhos = connection.Query<ConselhoQueryResult>(sql);

                    if (conselhos != null)
                    {
                        foreach (var item in conselhos)
                        {
                            var sqlUnidades = $@"SELECT 
	                                                 U.NomeFantasia as Nome
	                                                 ,E.Logradouro + ' - ' + E.Cidade + ' - ' + E.Uf as Endereco
	                                                 ,U.Telefone as Contato
	                                                 ,F.Nome AS NomeGestor
                                                     ,F.Login + '@sestsenat.org.br' as EmailGestor
                                                FROM {_context.PrefixGestaoV1}[Unidade] U 
                                                LEFT JOIN {_context.PrefixGestaoV1}[EnderecoMeio] EM ON U.IdUnidade = EM.IdUnidade
                                                LEFT JOIN {_context.PrefixGestaoV1}[Endereco] E ON E.IdEndereco = EM.IdEndereco
                                                LEFT JOIN {_context.PrefixGestaoV1}[Funcionario] F ON F.IdFuncinario = U.IdDiretor
                                                WHERE U.IdConselho = '{item.IdConselho}' AND U.Ativo = 1
                                                ORDER BY U.NomeFantasia";

                            item.Unidades = connection.Query<UnidadeQueryResult>(sqlUnidades);
                        }
                    }

                    return conselhos;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
