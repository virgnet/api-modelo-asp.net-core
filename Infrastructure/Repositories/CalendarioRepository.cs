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
    public class CalendarioRepository : ICalendarioRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public CalendarioRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public CalendarioQueryResult BuscarCalendario(Guid idCalendario)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"SELECT [IdCalendario]
                          ,[IdTema]
                          ,[Identificador]
                          ,[Local]
                          ,[Titulo]
                          ,[Descricao]
                          ,[Ativo]
                          ,[DataCadastro]
                          ,[DataInicio]
                          ,[DataFim]
                      FROM { _context.PrefixCMS }[Calendario]
                      WHERE IdCalendario = '{idCalendario}'";

                    connection.Open();

                    return connection.Query<CalendarioQueryResult>(sqlDocumentos).FirstOrDefault();
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<CalendarioQueryResult> ListarCalendario()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var sql = $@"SELECT [IdCalendario]
                          ,[IdTema]
                          ,[Identificador]
                          ,[Local]
                          ,[Titulo]
                          ,[Descricao]
                          ,[Ativo]
                          ,[DataCadastro]
                          ,[DataInicio]
                          ,[DataFim]
                      FROM { _context.PrefixCMS }[Calendario]
                      WHERE Ativo = 1";
                    return connection.Query<CalendarioQueryResult>(sql);
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<CalendarioQueryResult> PesquisarCalendario(Guid? idTema, string textoDaBusca, int? ano, int? mes)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string whereTema = "";
                    string whereAno = "";
                    string whereMes = "";

                    if (idTema.HasValue)
                        whereTema = $@" IdTema = '{ idTema }' AND ";

                    if (ano.HasValue)
                        whereAno = $@"  AND (YEAR(DataFim) = {ano} OR YEAR(DataInicio) = {ano})";

                    if (mes.HasValue)
                        whereMes = $@"  AND (MONTH(DataFim) = {mes} OR MONTH(DataFim) = {mes})";

                    var sqlDocumentos = $@"SELECT [IdCalendario]
                          ,[IdTema]
                          ,[Identificador]
                          ,[Local]
                          ,[Titulo]
                          ,[Descricao]
                          ,[Ativo]
                          ,[DataCadastro]
                          ,[DataInicio]
                          ,[DataFim]
                      FROM { _context.PrefixCMS }[Calendario]
                    WHERE {whereTema} Ativo = 1 AND Titulo LIKE '%{ textoDaBusca }%' AND Descricao LIKE '%{ textoDaBusca }%' AND Local LIKE '%{ textoDaBusca }%' 
                    {whereAno} {whereMes}
                    ";

                    connection.Open();

                    return connection.Query<CalendarioQueryResult>(sqlDocumentos);
                }
                catch
                {
                    return null;
                }
            }
        }
        public IEnumerable<DateTime> ListarDatas()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"SELECT DISTINCT DataInicio DataInicioEFim
                                 FROM { _context.PrefixCMS }[Calendario]
                                 WHERE Ativo = 1

				                 UNION 

				                 SELECT DISTINCT DataFim DataInicioEFim
                                 FROM { _context.PrefixCMS }[Calendario]
                                 WHERE Ativo = 1";

                    return connection.Query<DateTime>(sql);
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool SalvarCalendario(Calendario calendario)
        {
            try
            {
                var obj = _context.Calendario.Where(c => c.IdCalendario.Equals(calendario.IdCalendario)).FirstOrDefault();
                if (obj != null)
                {
                    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
                    {
                        try
                        {
                            connection.Open();
                            var sql = $@"UPDATE { _context.PrefixCMS }[Calendario]
                                       SET [IdTema] = @IdTema
                                          ,[Identificador] = @Identificador
                                          ,[Local] = @Local
                                          ,[Titulo] = @Titulo
                                          ,[Descricao] = @Descricao
                                          ,[DataInicio] = @DataInicio
                                          ,[DataFim] = @DataFim
                                   WHERE IdCalendario = @IdCalendario";

                            connection.Execute(sql, new
                            {
                                IdTema = calendario.IdTema,
                                Identificador = calendario.Identificador,
                                Local = calendario.Local,
                                Titulo = calendario.Titulo,
                                Descricao = calendario.Descricao,
                                DataInicio = calendario.DataInicio,
                                DataFim = calendario.DataFim,
                                IdCalendario = calendario.IdCalendario
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
                    var retorno = _context.Calendario.Add(calendario);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar Tema: " + ex.Message, ex);
            }

        }

        public bool DesativarCalendario(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"UPDATE { _context.PrefixCMS }Calendario SET Ativo = 0 WHERE IdCalendario = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool ExcluirCalendario(Guid idCalendario)
        {
            try
            {
                var obj = _context.Calendario.Where(c => c.IdCalendario.Equals(idCalendario)).FirstOrDefault();
                if (obj != null)
                {
                    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
                    {
                        try
                        {
                            connection.Open();
                            var sql = $@"UPDATE { _context.PrefixCMS }[Calendario]
                                                SET [Ativo] = 0
                                      WHERE IdCalendario = @IdCalendario";

                            connection.Execute(sql, new
                            {
                                IdCalendario = obj.IdCalendario
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
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao excluir Calendario: " + ex.Message, ex);
            }
        }
    }
}
