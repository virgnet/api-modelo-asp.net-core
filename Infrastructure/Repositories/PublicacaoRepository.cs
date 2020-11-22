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
        private readonly ModeloDataContext _context;

        public PublicacaoRepository(ModeloDataContext context)
        {
            _context = context;
        }

        public PublicacaoQueryResult Buscar(Guid idPublicacao)
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
                      FROM [Publicacao] AS P
                      INNER JOIN [Tema] AS T ON P.IdTema = T.IdTema
                      INNER JOIN [Indicador] AS I ON P.IdTipoDeConteudo = I.IdIndicador
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


        public IEnumerable<PublicacaoQueryResult> Pesquisar(Guid? idTema, string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string whereTema = "";
                    if (idTema.HasValue)
                        whereTema = $@" P.IdTema = '{ idTema }' AND ";


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
                      FROM [Publicacao] AS P
                      INNER JOIN [Tema] AS T ON P.IdTema = T.IdTema
                      INNER JOIN [Indicador] AS I ON P.IdTipoDeConteudo = I.IdIndicador
                    WHERE P.Ativo = 1 AND {whereTema} (P.Titulo LIKE '%{ textoDaBusca }%' OR P.Chamada LIKE '%{ textoDaBusca }%')
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

        public bool Salvar(Publicacao publicacao)
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
                            var sql = $@"UPDATE [Publicacao]
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
                                Identificador = publicacao.Identificador,
                                Conteudo = publicacao.Conteudo,
                                Ativo = publicacao.Ativo,
                                DataCadastro = publicacao.DataCadastro,
                                DataPublicacao = publicacao.DataPublicacao
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


        public bool Desativar(Publicacao obj)
        {
            throw new NotImplementedException();
        }
    }
}
