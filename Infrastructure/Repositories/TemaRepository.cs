using Dapper;
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
        private readonly ModeloDataContext _context;

        public TemaRepository(ModeloDataContext context)
        {
            _context = context;
        }

        public TemaQueryResult Buscar(Guid id)
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
                                FROM [Tema] AS T
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
                                    FROM [TemaRelacionamento] AS TR
                                    WHERE TR.IdTema = '{ tema.IdTema }'
                        ;";

                }
                    return tema;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<TemaQueryResult> Pesquisar(string textoDaBusca)
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
                                FROM [Tema] AS T
                                WHERE T.[Ativo] = 1 AND (T.Tags like '%{ textoDaBusca }%' OR T.Titulo like '%{ textoDaBusca }%')
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

        public IEnumerable<TemaQueryResult> PesquisarTodos()
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
                                FROM [Tema] AS T
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

        public bool Desativar(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"UPDATE Tema SET Ativo = 0 WHERE IdTema = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Excluir(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"DELETE TemaRelacionamento WHERE IdTemaRelacionamento = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Salvar(Tema tema)
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
                            var sql = $@"UPDATE [Tema]
                                   SET [Titulo] = @Titulo
                                   ,[Tags] = @Tags
                                   ,[Descricao] = @Descricao
                                   ,[Imagem] = @Imagem
                                   WHERE IdTema = @IdTema";

                            connection.Execute(sql, new
                            {
                                Titulo = tema.Titulo,
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
    }
}
