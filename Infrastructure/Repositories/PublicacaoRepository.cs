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
    public class PublicacaoRepository : IPublicacaoRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public PublicacaoRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public PublicacaoQueryResult BuscarPublicacao(Guid idPublicacao)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"SELECT P.IdPublicacao
                          ,P.IdTema
                          ,P.IdTipoDeConteudo
                          ,P.Identificador
                          ,P.Titulo
                          ,P.Chamada
                          ,P.Conteudo
                          ,P.Tags
                          ,P.Ativo
                          ,P.DataCadastro
                          ,P.DataPublicacao
	                     ,T.Titulo AS Tema
	                     ,I.Titulo AS TipoDeConteudo
                      FROM { _context.PrefixCMS }[Publicacao] AS P
                      INNER JOIN { _context.PrefixCMS }[Tema] AS T ON P.IdTema = T.IdTema
                      INNER JOIN { _context.PrefixCMS }[Indicador] AS I ON P.IdTipoDeConteudo = I.IdIndicador
                    WHERE P.IdPublicacao = '{ idPublicacao }'";

                    connection.Open();

                    var contato = connection.Query<PublicacaoQueryResult>(sqlDocumentos).FirstOrDefault();

                    return contato;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<TipoDeConteudoQueryResult> ListarTipoDeConteudo()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var sql = $@"SELECT IdIndicador AS IdIndicador, Titulo FROM { _context.PrefixCMS }[Indicador] WHERE IdTipoIndicador = '8520C92D-021C-4332-BE5B-904730C0D610' ORDER BY 2";
                    return connection.Query<TipoDeConteudoQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<PublicacaoQueryResult> PesquisarPublicacao(Guid? idTipoDeConteudo, Guid? idTema, string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string whereTema = "";
                    if (idTema.HasValue)
                        whereTema = $@" P.IdTema = '{ idTema }' AND ";

                    string whereTipoConteudo = "";
                    if (idTipoDeConteudo.HasValue)
                        whereTipoConteudo = $@" P.IdTipoDeConteudo = '{ idTipoDeConteudo }' AND ";

                    var sqlDocumentos = $@"
                    SELECT P.IdPublicacao
                          ,P.IdTema
                          ,P.IdTipoDeConteudo
                          ,P.Identificador
                          ,P.Titulo
                          ,P.Chamada
                          ,P.Conteudo
                          ,P.Tags
                          ,P.Ativo
                          ,P.DataCadastro
                          ,P.DataPublicacao
	                     ,T.Titulo AS Tema
	                     ,I.Titulo AS TipoDeConteudo
                      FROM { _context.PrefixCMS }[Publicacao] AS P
                      INNER JOIN { _context.PrefixCMS }[Tema] AS T ON P.IdTema = T.IdTema
                      INNER JOIN { _context.PrefixCMS }[Indicador] AS I ON P.IdTipoDeConteudo = I.IdIndicador
                    WHERE P.Ativo = 1 AND {whereTipoConteudo} {whereTema} (P.Titulo LIKE '%{ textoDaBusca }%' OR P.Chamada LIKE '%{ textoDaBusca }%')
                    ORDER BY P.DataPublicacao DESC";

                    connection.Open();

                    var contato = connection.Query<PublicacaoQueryResult>(sqlDocumentos);

                    return contato;
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool SalvarPublicacao(Publicacao publicacao)
        {
            try
            {
                var obj = _context.Publicacao.Where(c => c.IdPublicacao.Equals(publicacao.IdPublicacao)).FirstOrDefault();
                if (obj != null)
                {
                    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
                    {
                        try
                        {
                            connection.Open();
                            var sql = $@"UPDATE { _context.PrefixCMS }[Publicacao]
                                         SET [IdTema] = @IdTema
                                            ,[IdTipoDeConteudo] = @IdTipoDeConteudo
                                            ,[Identificador] = @Identificador
                                            ,[Titulo] = @Titulo
                                            ,[Chamada] = @Chamada
                                            ,[Conteudo] = @Conteudo
                                            ,[Tags] = @Tags
                                            ,[Ativo] = @Ativo
                                            ,[DataCadastro] = @DataCadastro
                                            ,[DataPublicacao] = @DataPublicacao
                                            ,[Binario] = @Binario
                                            ,[ImagemCapa] = @ImagemCapa
                                         WHERE [IdPublicacao] = @IdPublicacao
                                        ";

                            connection.Execute(sql, new
                            {
                                IdTema = publicacao.IdTema,
                                IdTipoDeConteudo = publicacao.IdTipoDeConteudo,
                                Identificador = publicacao.Identificador,
                                Chamada = publicacao.Chamada,
                                Conteudo = publicacao.Conteudo,
                                Tags = publicacao.Tags,
                                Ativo = publicacao.Ativo,
                                DataCadastro = publicacao.DataCadastro,
                                DataPublicacao = publicacao.DataPublicacao,
                                Binario = publicacao.Binario,
                                ImagemCapa = publicacao.ImagemCapa
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
                    var retorno = _context.Publicacao.Add(publicacao);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar Tema: " + ex.Message, ex);
            }
        }

        public bool SalvarArquivos(Publicacao publicacao)
        {

            try
            {
                var obj = _context.Publicacao.Where(c => c.IdPublicacao.Equals(publicacao.IdPublicacao)).FirstOrDefault();
                if (obj != null)
                {
                    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
                    {
                        connection.Open();
                        if (publicacao.ImagemCapa != null)
                        {                         
                            var sql = $@"UPDATE { _context.PrefixCMS }[Publicacao]
                                         SET   [ImagemCapa] = @ImagemCapa
                                         WHERE [IdPublicacao] = @IdPublicacao
                                        ";

                            connection.Execute(sql, 
                                new {
                                    ImagemCapa = publicacao.ImagemCapa,
                                    IdPublicacao = publicacao.IdPublicacao
                                });
                        } 

                        if (publicacao.Binario != null)
                        {
                            var sql = $@"UPDATE { _context.PrefixCMS }[Publicacao]
                                         SET   [Binario] = @Binario
                                         WHERE [IdPublicacao] = @IdPublicacao
                                        ";

                            connection.Execute(sql,
                                new
                                {
                                    Binario = publicacao.Binario,
                                    IdPublicacao = publicacao.IdPublicacao
                                });
                        }
                       

                        try
                        {
                            
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    }                    

                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar Tema: " + ex.Message, ex);
            }
        }
    }
}
