using Dapper;
using Domain.Commands;
using Domain.Entities;
using Domain.Queries;
using Domain.Repositories;
using Infrastructure.DataContext;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class TemaRepository : ITemaRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public TemaRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public TemaQueryResult BuscarTema(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlTema = $@"
                                SELECT T.[IdTema]
                                      ,T.[Titulo]
                                      ,T.[Tags]
                                      ,T.[Descricao]
                                      ,T.[Imagem]
                                      ,T.[Ativo]
                                FROM { _context.PrefixCMS }[Tema] AS T
                                WHERE T.IdTema = '{ id }'
                    ;";

                    connection.Open();

                    var tema = connection.Query<TemaQueryResult>(sqlTema).FirstOrDefault();

                    if (tema != null)
                    {
                        var sqlRelacionamentos = $@"
                                    SELECT TR.[IdTemaRelacionamento]
                                          ,TR.[IdTema]
                                          ,TR.[IdArea]
                                          ,TR.[IdProjeto]
                                          ,TR.[IdSistema]
                                          ,TR.[IdDocumento]
                                    FROM { _context.PrefixCMS }[TemaRelacionamento] AS TR
                                    WHERE TR.IdTema = '{ tema.IdTema }'
                        ;";

                        tema.Relacionamentos = connection.Query<TemaRelacionamentosQueryResult>(sqlRelacionamentos).ToList();

                        var sqlDocumentos = $@"
                                            SELECT 
                                                D.[IdDocumento] AS IdDocumento
                                                ,T.[Titulo] + ' - ' + D.Titulo AS Titulo
                                            FROM
                                                { _context.PrefixCMS }[TemaRelacionamento] TR
                                            INNER JOIN
                                                { _context.PrefixSIRDOC }[Documento] AS D ON TR.IdDocumento = D.IdDocumento
                                            INNER JOIN { _context.PrefixSIRDOC }[Indicador] AS T ON T.IdIndicador = D.IdTipoDocumento
                                            WHERE TR.IdTema = '{ tema.IdTema }'                        
                                            ;";

                        tema.Documentos = connection.Query<DocumentoQueryResult>(sqlDocumentos).ToList();

                        var sqlAreas = $@"
                                        SELECT 
                                            C.[IdCoordenacao] AS IdArea
                                            ,ISNULL(C.[Sigla] + ' - ', '') + C.[Nome] AS Titulo
                                        FROM
                                            TemaRelacionamento TR
                                        INNER JOIN { _context.PrefixGestaoV1 }[Coordenacao] AS C ON C.IdCoordenacao = TR.IdArea
                                        WHERE TR.IdTema = '{ tema.IdTema }'                        
                                        ;";

                        tema.Areas = connection.Query<AreaQueryResult>(sqlAreas).ToList();

                        var sqlProjetos = $@"
                                            SELECT 
                                                P.[IdPublicacao] AS IdProjeto
                                                ,P.[Titulo] AS Titulo
                                            FROM
                                                TemaRelacionamento TR
                                            INNER JOIN { _context.PrefixCMS }[Publicacao] AS P ON P.IdPublicacao = TR.IdProjeto
                                            WHERE TR.IdTema = '{ tema.IdTema }'                        
                                            ;";

                        tema.Projetos = connection.Query<ProjetoQueryResult>(sqlProjetos).ToList();

                        var sqlSistemas = $@"
                                            SELECT 
                                               S.[IdSistema] AS IdSistema
                                                ,S.[Nome] AS Titulo
                                            FROM
                                                TemaRelacionamento TR
                                            INNER JOIN { _context.PrefixGestaoV1 }[Sistema] AS S ON S.IdSistema = TR.IdSistema
                                            WHERE TR.IdTema = '{ tema.IdTema }'                        
                                            ;";

                        tema.Sistemas = connection.Query<SistemaQueryResult>(sqlSistemas).ToList();

                        var sqlComunicados = $@"SELECT C.IdComunicado
                                    ,C.DataCadastro
                                    ,C.DataPublicacao
                                    ,C.DestinatarioDex
                                    ,C.DestinatarioUnidade
	                                ,CV.Assunto
	                                ,CV.Conteudo
	                                ,CV.IdTemplate
	                                ,T1.Titulo AS Template
	                                ,CV.Versao
                                FROM { _context.PrefixCMS }[Comunicado] AS C
                                INNER JOIN { _context.PrefixCMS }[ComunicadoVersao] AS CV ON C.IdComunicado = CV.IdComunicado AND CV.Versao = (SELECT MAX(CMAX.Versao) FROM { _context.PrefixCMS }[ComunicadoVersao] AS CMAX WHERE CMAX.IdComunicado = C.IdComunicado) 
                                LEFT JOIN { _context.PrefixCMS }[Indicador] AS T1 ON CV.IdTemplate = T1.IdIndicador
                                WHERE CV.IdTema = '{ tema.IdTema }'
                                    AND C.Ativo = 1 
                            ;";

                        tema.Comunicados = connection.Query<ComunicadoQueryResult>(sqlComunicados).ToList();

                        var sqlCalendarios = $@"SELECT C.IdTema
                                                ,C.IdCalendario
                                            FROM Calendario C
                                            INNER JOIN Tema T ON C.IdTema = T.IdTema
                                            WHERE C.IdTema = '{ tema.IdTema }'
                        ;";

                        tema.Calendarios = connection.Query<CalendarioQueryResult>(sqlCalendarios).ToList();
                }
                    return tema;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<TemaQueryResult> PesquisarTema(string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    // CAMPO IMAGEM
                    //var sqlDocumentos = $@"
                    //            SELECT T.[IdTema]
                    //                  ,T.[Titulo]
                    //                  ,T.[Tags]
                    //                  ,T.[Descricao]
                    //                  ,T.[Imagem]
                    //                  ,T.[Ativo]
                    //            FROM { _context.PrefixCMS }[Tema] AS T
                    //            WHERE T.[Ativo] = 1 AND (T.Tags like '%{ textoDaBusca }%' OR T.Titulo like '%{ textoDaBusca }%')
                    //            ORDER BY T.[Titulo]";

                    var sqlDocumentos = $@"
                                SELECT T.[IdTema]
                                      ,T.[Titulo]
                                      ,T.[Tags]
                                      ,T.[Descricao]
                                      ,T.[Ativo]
                                FROM { _context.PrefixCMS }[Tema] AS T
                                WHERE T.[Ativo] = 1 AND (T.Tags like '%{ textoDaBusca }%' OR T.Titulo like '%{ textoDaBusca }%')
                                ORDER BY T.[Titulo]";

                    connection.Open();
                    var temas = connection.Query<TemaQueryResult>(sqlDocumentos);
                    foreach (var tema in temas)
                    {
                        var sqlRelacionamentos = $@"SELECT TR.[IdTemaRelacionamento]
                                          ,TR.[IdTema]
                                          ,TR.[IdArea]
                                          ,TR.[IdProjeto]
                                          ,TR.[IdSistema]
                                          ,TR.[IdDocumento]
                                    FROM { _context.PrefixCMS }[TemaRelacionamento] AS TR
                                WHERE TR.IdTema = '{ tema.IdTema }'";

                        tema.Relacionamentos = connection.Query<TemaRelacionamentosQueryResult>(sqlRelacionamentos).ToList();
                    }
                    return temas;
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<TemaQueryResult> PesquisarTodosTemas()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"
                                SELECT T.[IdTema]
                                      ,T.[Titulo]
                                      ,T.[Tags]
                                      ,T.[Descricao]
                                      ,T.[Ativo]
                                FROM { _context.PrefixCMS }[Tema] AS T
                                WHERE T.Ativo = 1
                                ORDER BY T.[Titulo]";

                    connection.Open();
                    var temas = connection.Query<TemaQueryResult>(sqlDocumentos);
                    return temas;
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool DesativarTema(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"UPDATE { _context.PrefixCMS }Tema SET Ativo = 0 WHERE IdTema = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ExcluirTemaRelacionamento(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"DELETE { _context.PrefixCMS }TemaRelacionamento WHERE IdTemaRelacionamento = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SalvarTema(Tema tema)
        {
            try
            {
                var obj = _context.Tema.Where(c => c.IdTema.Equals(tema.IdTema)).FirstOrDefault();
                if (obj != null)
                {
                    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
                    {
                        try
                        {
                            connection.Open();
                            var sql = $@"UPDATE { _context.PrefixCMS }[Tema]
                                   SET [Titulo] = @Titulo
                                   ,[Tags] = @Tags
                                   ,[Descricao] = @Descricao
                                   ,[Imagem] = @Imagem
                                   WHERE IdTema = @IdTema";

                            connection.Execute(sql, new
                            {
                                Titulo = tema.Titulo,
                                Tags = tema.Tags,
                                Descricao = tema.Descricao,
                                Imagem = tema.Imagem,
                                IdTema = tema.IdTema
                            });
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    var retorno = _context.Tema.Add(tema);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar Tema: " + ex.Message, ex);
            }
            
        }

        public bool SalvarTemaRelacionamento(TemaRelacionamento relacionamento)
        {
            try
            {
                var retorno = _context.TemaRelacionamento.Add(relacionamento);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Relacionamento: " + ex.Message, ex);
            }
        }

        public IEnumerable<AreaQueryResult> PesquisarArea(string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"
                                SELECT [IdCoordenacao] AS IdArea
                                    ,ISNULL([Sigla] + ' - ', '') + [Nome] AS Titulo
                                    ,(CASE WHEN DepartamentoExecutivo = 1 THEN 'dex' ELSE 'undiade' END) AS Origem
                                FROM { _context.PrefixGestaoV1 }[Coordenacao]
                                WHERE Ativo = 1 AND Nome like '%{ textoDaBusca }%'
                                ORDER BY Origem, Titulo";

                    connection.Open();
                    return connection.Query<AreaQueryResult>(sqlDocumentos);
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<SistemaQueryResult> PesquisarSistema(string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"
                                SELECT[IdSistema] AS IdSistema
                                    ,[Nome] AS Titulo
                                FROM { _context.PrefixGestaoV1 }[Sistema]
                                WHERE Ativo = 1 AND Nome LIKE '%{ textoDaBusca }%'
                                ORDER BY Nome";

                    connection.Open();
                    return connection.Query<SistemaQueryResult>(sqlDocumentos);
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<DocumentoQueryResult> PesquisarDocumento(string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"
                                    SELECT [IdDocumento] AS IdDocumento
                                        ,T.[Titulo] + ' - ' + D.Titulo AS Titulo
                                    FROM { _context.PrefixSIRDOC }[Documento] AS D
                                    INNER JOIN { _context.PrefixSIRDOC }[Indicador] AS T ON T.IdIndicador = D.IdTipoDocumento
                                    WHERE D.Ativo = 1 AND (D.Titulo like '%{ textoDaBusca }%' OR D.Tags like '%{ textoDaBusca }%')
                                    ORDER BY D.Titulo";

                    connection.Open();
                    return connection.Query<DocumentoQueryResult>(sqlDocumentos);
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<ProjetoQueryResult> PesquisarProjeto(string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"
                                    SELECT [IdPublicacao] AS IdProjeto
                                        ,[Titulo] AS Titulo
                                    FROM { _context.PrefixCMS }[Publicacao]
                                    WHERE IdTipoDeConteudo = '4D711626-21F0-4B74-BE45-925F41D70A82' AND Ativo = 1 AND (Titulo like '%{ textoDaBusca }%' OR Tags like '%{ textoDaBusca }%')";

                    connection.Open();
                    return connection.Query<ProjetoQueryResult>(sqlDocumentos);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
