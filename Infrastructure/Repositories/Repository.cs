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
    public class Repository //: IRepository
    {
        //private readonly PortalDoColaboradorDataContext _context;

        //public Repository(PortalDoColaboradorDataContext context)
        //{
        //    _context = context;
        //}

        //public IEnumerable<CalendarioQueryResult> PesquisarDocumento(Guid idTipoDocumento, string textoDaBusca)
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            var sqlDocumentos = $@"SELECT D.[IdDocumento] AS IdDocumento
        //                        , D.[IdTipoDocumento] AS IdTipoDocumento
	       //                     , I.Titulo AS TipoDocumento
        //                        , D.[Ativo] AS Ativo
        //                        , D.[IdUnidadeSIGOP] AS IdUnidadeSIGOP
        //                        , U.Nome AS UnidadeSIGOP
        //                        , D.[Titulo] AS Titulo
        //                        , D.[Descricao] AS Descricao
        //                        , D.[Tags] AS Tags
        //                        , D.[Criador] AS Criador
        //                        , D.[DataCadastro] AS DataCadastro
        //                        FROM       { _context.PrefixSIRDOC }[Documento] AS D
        //                        INNER JOIN { _context.PrefixSIRDOC }[Indicador] AS I ON D.IdTipoDocumento = I.IdIndicador
        //                        INNER JOIN { _context.PrefixSIGOP }[Unidade] AS U ON U.IdUnidade = D.IdUnidadeSIGOP
        //                        WHERE D.Ativo = 1 AND (D.Tags like '%{ textoDaBusca }%' OR D.Titulo like '%{ textoDaBusca }%' OR D.Descricao like '%{ textoDaBusca }%' OR U.Nome like '%{ textoDaBusca }%')
        //                        AND IdTipoDocumento = '{idTipoDocumento}'";

        //            connection.Open();
        //            var documetos = connection.Query<CalendarioQueryResult>(sqlDocumentos);
        //            foreach (var documento in documetos)
        //            {
        //                var sqlArquivos = $@"SELECT A.[IdArquivo] AS IdArquivo
        //                        ,A.[IdArquivoPai] AS IdArquivoPai
        //                        ,A.[IdDocumento] AS IdDocumento
        //                        ,A.[Ativo] AS Ativo
        //                        ,A.[Versao] AS Versao
        //                        ,A.[Ordem] AS Ordem
        //                        ,A.[Tipo] AS Tipo
        //                        ,A.[Nome] AS Nome
        //                        ,A.[Criador] AS Criador
        //                        ,A.[DataUpload] AS DataUpload
        //                        FROM { _context.PrefixSIRDOC }[Arquivo] AS A
        //                        WHERE A.Ativo = 1 AND A.IdDocumento = '{ documento.IdDocumento }'";

        //                documento.Arquivos = connection.Query<ArquivoQueryResult>(sqlArquivos).ToList();
        //            }
        //            return documetos;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public CalendarioQueryResult BuscarDocumento(Guid idDocumento)
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            var sqlDocumentos = $@"SELECT D.[IdDocumento] AS IdDocumento
        //                        , D.[IdTipoDocumento] AS IdTipoDocumento
	       //                     , I.Titulo AS TipoDocumento
        //                        , D.[Ativo] AS Ativo
        //                        , D.[IdUnidadeSIGOP] AS IdUnidadeSIGOP
        //                        , U.Nome AS UnidadeSIGOP
        //                        , D.[Titulo] AS Titulo
        //                        , D.[Descricao] AS Descricao
        //                        , D.[Tags] AS Tags
        //                        , D.[Criador] AS Criador
        //                        , D.[DataCadastro] AS DataCadastro
        //                        FROM       { _context.PrefixSIRDOC }[Documento] AS D
        //                        INNER JOIN { _context.PrefixSIRDOC }[Indicador] AS I ON D.IdTipoDocumento = I.IdIndicador
        //                        INNER JOIN { _context.PrefixSIGOP }[Unidade] AS U ON U.IdUnidade = D.IdUnidadeSIGOP
        //                        WHERE D.IdDocumento = '{ idDocumento }'";

        //            connection.Open();  
                    
        //            var documento = connection.Query<CalendarioQueryResult>(sqlDocumentos).FirstOrDefault();

        //            if(documento != null) { 
        //                var sqlArquivos = $@"SELECT A.[IdArquivo] AS IdArquivo
        //                            ,A.[IdArquivoPai] AS IdArquivoPai
        //                            ,A.[IdDocumento] AS IdDocumento
        //                            ,A.[Ativo] AS Ativo
        //                            ,A.[Versao] AS Versao
        //                            ,A.[Ordem] AS Ordem
        //                            ,A.[Tipo] AS Tipo
        //                            ,A.[Nome] AS Nome
        //                            ,A.[Criador] AS Criador
        //                            ,A.[DataUpload] AS DataUpload
        //                            FROM { _context.PrefixSIRDOC }[Arquivo] AS A
        //                            WHERE A.Ativo = 1 AND A.IdDocumento = '{ documento.IdDocumento }'";

        //                documento.Arquivos = connection.Query<ArquivoQueryResult>(sqlArquivos).ToList();
        //            }

        //            return documento;
        //        }
        //        catch(Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public bool SalvarTipoDocumento(Indicador tipo)
        //{
        //    try
        //    {
        //        var retorno = _context.Indicador.Add(tipo);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao inserir Inscricao: " + ex.Message, ex);
        //    }
        //}

        //public bool SalvarDocumento(Old_Documento documento)
        //{
        //    try
        //    {
        //        var doc = _context.Documento.Where(c => c.IdDocumento.Equals(documento.IdDocumento)).FirstOrDefault();
        //        if(doc != null) {
        //            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //            {
        //                try
        //                {
        //                    connection.Open();
        //                    var sql = $@"UPDATE { _context.PrefixSIRDOC }[Documento]
        //                           SET [IdTipoDocumento] = @IdTipoDocumento
        //                           ,[IdUnidadeSIGOP] = @IdUnidadeSIGOP
        //                           ,[Titulo] = @Titulo
        //                           ,[Descricao] = @Descricao
        //                           ,[Tags] = @Tags
        //                           WHERE IdDocumento = @IdDocumento";

        //                    connection.Execute(sql, new
        //                    {
        //                        IdTipoDocumento = documento.IdTipoDocumento,
        //                        IdUnidadeSIGOP = documento.IdUnidadeSIGOP,
        //                        Titulo = documento.Titulo,
        //                        Descricao = documento.Descricao,
        //                        Tags = documento.Tags,
        //                        IdDocumento = documento.IdDocumento
        //                    });
        //                }
        //                catch (Exception ex)
        //                {
        //                    return false;
        //                }
        //            }
        //            return true;
        //        }
        //        else {
        //            var retorno = _context.Documento.Add(documento);
        //            _context.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao inserir Inscricao: " + ex.Message, ex);
        //    }
        //}

        //public bool SalvarArquivo(Arquivo arquivo)
        //{
        //    try
        //    {
        //        var retorno = _context.Arquivo.Add(arquivo);
        //        _context.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Erro ao inserir Inscricao: " + ex.Message, ex);
        //    }
        //}

        //public Byte[] DownloadArquivo(Guid idArquivo)
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            var sql = $@"SELECT Binario FROM  { _context.PrefixSIRDOC }[Arquivo] WHERE IdArquivo = '{ idArquivo }'";
        //            var arquivo = connection.Query<Arquivo>(sql).FirstOrDefault();
        //            if (arquivo != null)
        //                return arquivo.Binario;
        //            else
        //                return null;
        //        }
        //        catch
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public bool DesativarDocumento(Guid idDocumento)
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            var sql = $@"UPDATE { _context.PrefixSIRDOC }Documento SET Ativo = 0 WHERE IdDocumento = '{ idDocumento }'";
        //            connection.Open();
        //            return connection.Query<bool>(sql).FirstOrDefault();
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public bool DesativarArquivo(Guid idArquivo)
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            var sql = $@"UPDATE { _context.PrefixSIRDOC }Arquivo SET Ativo = 0 WHERE IdArquivo = '{ idArquivo }'";
        //            connection.Open();
        //            return connection.Query<bool>(sql).FirstOrDefault();
        //        }
        //        catch
        //        {
        //            return false;
        //        }
        //    }
        //}

        //public IEnumerable<UnidadeQueryResult> ListarUnidade()
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            var sql = $@"SELECT [IdUnidade], [Nome] FROM { _context.PrefixSIGOP }[Unidade] ORDER BY 2";
        //            return connection.Query<UnidadeQueryResult>(sql);
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }
        //}

        //public IEnumerable<TipoDeConteudoQueryResult> ListarTipoDocummento()
        //{
        //    using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            var sql = $@"SELECT IdIndicador AS IdTipoDocumento, Titulo FROM { _context.PrefixSIRDOC }[Indicador] WHERE IdTipoIndicador = '344A71DD-56AB-4F42-83C4-80B2AB356417' ORDER BY 2";
        //            return connection.Query<TipoDeConteudoQueryResult>(sql);
        //        }
        //        catch (Exception ex)
        //        {
        //            return null;
        //        }
        //    }
        //}
    }
}
