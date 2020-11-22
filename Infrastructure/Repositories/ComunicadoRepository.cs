using Dapper;
using Domain.Entities;
using Domain.Queries;
using Domain.Repositories;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infrastructure.Repositories
{
    public class ComunicadoRepository : IComunicadoRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public ComunicadoRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public IEnumerable<TemplateComunicadoQueryResult> ListarTemplateComunicado()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var sql = $@"SELECT IdIndicador AS IdTemplate, Titulo FROM { _context.PrefixCMS }[Indicador] WHERE IdTipoIndicador = 'BF2F6CA2-0FF6-4906-B55C-AADA6FCA67D7' ORDER BY 2";
                    return connection.Query<TemplateComunicadoQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public int BuscarVersaoComunicado(Guid idComunicado)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"SELECT ISNULL(MAX(CMAX.Versao), 0) FROM { _context.PrefixCMS }[ComunicadoVersao] AS CMAX WHERE CMAX.IdComunicado = '{ idComunicado }'";

                    connection.Open();

                    var documento = connection.Query<int>(sqlDocumentos).FirstOrDefault();

                    return documento;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }

        public ComunicadoQueryResult BuscarComunicado(Guid idComunicado)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"SELECT C.IdComunicado
                        ,C.DataCadastro
                        ,C.DataPublicacao
                        ,C.Ativo
                        ,C.DestinatarioDex
                        ,C.DestinatarioUnidade
                        ,C.DestinatarioDexCoordenador
                        ,C.DestinatarioUnidadeDiretor
                        ,C.DestinatarioUnidadeSupervisor
                        ,C.DestinatarioUnidadeCaf
                        ,C.DestinatarioUnidadeCoordenadorSest
                        ,C.DestinatarioUnidadeCoordenadorSenat
	                    ,CV.Assunto
	                    ,CV.Conteudo
	                    ,CV.IdTema
	                    ,T2.Titulo AS Tema
	                    ,CV.IdTemplate
	                    ,T1.Titulo AS Template
	                    ,CV.Versao
                    FROM { _context.PrefixCMS }[Comunicado] AS C
                    INNER JOIN { _context.PrefixCMS }[ComunicadoVersao] AS CV ON C.IdComunicado = CV.IdComunicado AND CV.Versao = (SELECT MAX(CMAX.Versao) FROM { _context.PrefixCMS }[ComunicadoVersao] AS CMAX WHERE CMAX.IdComunicado = C.IdComunicado) 
                    LEFT JOIN { _context.PrefixCMS }[Indicador] AS T1 ON CV.IdTemplate = T1.IdIndicador
                    LEFT JOIN { _context.PrefixCMS }[Tema] AS T2 ON CV.IdTema = T2.IdTema
                    WHERE C.IdComunicado = '{ idComunicado }' AND C.Ativo = 1";

                    connection.Open();

                    var documento = connection.Query<ComunicadoQueryResult>(sqlDocumentos).FirstOrDefault();

                    return documento;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ComunicadoQueryResult> PesquisarComunicado(Guid? idTema, Guid? idTemplate, string textoDaBusca, string contexto, string login, bool publicados)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string wherePublicados = "";
                    string orderByPublicados = "";
                    if (publicados)
                    {
                        wherePublicados = $@" C.DataPublicacao <= GETDATE() AND CV.Aprovado = 1 AND ";
                        orderByPublicados = $@" (CASE WHEN L.DataLeitura IS NULL THEN 0 ELSE 1 END), ";
                    }

                    string whereTema = "";
                    if (idTema.HasValue)
                        whereTema = $@" CV.IdTema = '{ idTema }' AND ";

                    string whereTemplate = "";
                    if (idTemplate.HasValue)
                        whereTemplate = $@" CV.IdTemplate = '{ idTemplate }' AND ";

                    string whereContexto = "";
                    switch (contexto)
                    {
                        case "diretor-dex":
                            whereContexto = "";
                            break;
                        case "dex":
                            whereContexto = $@" C.DestinatarioDex = 1 AND ";
                            break;
                        case "dex-coordenador":
                            whereContexto = $@" (C.DestinatarioDexCoordenador = 1 OR C.DestinatarioDex = 1) AND ";
                            break;
                        case "unidade":
                            whereContexto = $@" C.DestinatarioUnidade = 1 AND ";
                            break;
                        case "unidade-caf":
                            whereContexto = $@" (C.DestinatarioUnidadeCaf = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-coordenador-sest":
                            whereContexto = $@" (C.DestinatarioUnidadeCoordenadorSest = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-coordenador-senat":
                            whereContexto = $@" (C.DestinatarioUnidadeCoordenadorSenat = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-diretor":
                            whereContexto = $@" (C.DestinatarioUnidadeDiretor = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-supervisor":
                            whereContexto = $@" (C.DestinatarioUnidadeSupervisor = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                    }

        var sqlDocumentos = $@"SELECT TOP 20 C.IdComunicado
                        ,C.DataCadastro
                        ,C.DataPublicacao
                        ,C.Ativo
                        ,C.DestinatarioDex
                        ,C.DestinatarioUnidade
                        ,C.DestinatarioDexCoordenador
                        ,C.DestinatarioUnidadeDiretor
                        ,C.DestinatarioUnidadeSupervisor
                        ,C.DestinatarioUnidadeCaf
                        ,C.DestinatarioUnidadeCoordenadorSest
                        ,C.DestinatarioUnidadeCoordenadorSenat
	                    ,CV.Assunto
	                    ,CV.CriadoPor
	                    ,CV.Aprovado
	                    ,CV.Conteudo
	                    ,CV.IdTema
	                    ,T2.Titulo AS Tema
	                    ,CV.IdTemplate
	                    ,T1.Titulo AS Template
	                    ,CV.Versao AS Versao
                        ,L.DataLeitura AS DataLeitura
                        ,CV.IdComunicadoVersao AS IdComunicadoVersao
                    FROM { _context.PrefixCMS }[Comunicado] AS C
                    INNER JOIN { _context.PrefixCMS }[ComunicadoVersao] AS CV ON C.IdComunicado = CV.IdComunicado AND CV.Versao = (SELECT MAX(CMAX.Versao) FROM { _context.PrefixCMS }[ComunicadoVersao] AS CMAX WHERE CMAX.IdComunicado = C.IdComunicado) 
                    LEFT JOIN { _context.PrefixCMS }[Indicador] AS T1 ON CV.IdTemplate = T1.IdIndicador
                    LEFT JOIN { _context.PrefixCMS }[Tema] AS T2 ON CV.IdTema = T2.IdTema
                    LEFT JOIN { _context.PrefixCMS }[ComunicadoVersaoLog] AS L ON CV.IdComunicadoVersao = L.IdComunicadoVersao AND L.Login = '{login}'
                    WHERE {whereContexto} {whereTema} {whereTemplate} {wherePublicados} C.Ativo = 1 AND CV.Ativo = 1 AND (CV.Assunto LIKE '%{ textoDaBusca }%' OR CV.Conteudo LIKE '%{ textoDaBusca }%')
                    ORDER BY { orderByPublicados } CV.Aprovado, C.DataPublicacao DESC";

                    connection.Open();

                    var comunicados = connection.Query<ComunicadoQueryResult>(sqlDocumentos);

                    if (publicados)
                    {
                        foreach (var item in comunicados)
                        {
                            if (item.Conteudo.IndexOf("[video-inicio]") > 0 && item.Conteudo.IndexOf("[video-fim]") > 0)
                            {
                                int videoIni = item.Conteudo.IndexOf("[video-inicio]");
                                int videoFim = item.Conteudo.IndexOf("[video-fim]") + 11;

                                string videoOrigemSrc = item.Conteudo.Substring(videoIni + 14, (videoFim - videoIni) - 25);
                                string videoOrigem = item.Conteudo.Substring(videoIni, (videoFim - videoIni));
                                string videoDestino = item.Conteudo.Replace(videoOrigem, "<video controls src='" +
                                    videoOrigemSrc
                                    + "' width='640' height='480'></video>");
                                item.Conteudo = videoDestino;
                            }
                        }
                    }

                    return comunicados;
                }
                catch
                {
                    return null;
                }
            }
        }

        public IEnumerable<ComunicadoQueryResult> PesquisarComunicadoBanner(string contexto, string login)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string whereContexto = "";
                    switch (contexto)
                    {
                        case "dex":
                            whereContexto = $@" C.DestinatarioDex = 1 AND ";
                            break;
                        case "dex-coordenador":
                            whereContexto = $@" (C.DestinatarioDexCoordenador = 1 OR C.DestinatarioDex = 1) AND ";
                            break;
                        case "unidade":
                            whereContexto = $@" C.DestinatarioUnidade = 1 AND ";
                            break;
                        case "unidade-caf":
                            whereContexto = $@" (C.DestinatarioUnidadeCaf = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-coordenador-sest":
                            whereContexto = $@" (C.DestinatarioUnidadeCoordenadorSest = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-coordenador-senat":
                            whereContexto = $@" (C.DestinatarioUnidadeCoordenadorSenat = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-diretor":
                            whereContexto = $@" (C.DestinatarioUnidadeDiretor = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                        case "unidade-supervisor":
                            whereContexto = $@" (C.DestinatarioUnidadeSupervisor = 1 OR C.DestinatarioUnidade = 1) AND ";
                            break;
                    }

                    var sqlDocumentos = $@"SELECT TOP 3 C.IdComunicado
                        ,C.DataCadastro
                        ,C.DataPublicacao
                        ,C.Ativo
                        ,C.DestinatarioDex
                        ,C.DestinatarioUnidade
                        ,C.DestinatarioDexCoordenador
                        ,C.DestinatarioUnidadeDiretor
                        ,C.DestinatarioUnidadeSupervisor
                        ,C.DestinatarioUnidadeCaf, 
                        ,C.DestinatarioUnidadeCoordenadorSest, 
                        ,C.DestinatarioUnidadeCoordenadorSenat,
	                    ,CV.Assunto
	                    ,CV.CriadoPor
	                    ,CV.Aprovado
	                    ,CV.Conteudo
	                    ,CV.IdTema
	                    ,T2.Titulo AS Tema
	                    ,CV.IdTemplate
	                    ,T1.Titulo AS Template
	                    ,CV.Versao AS Versao
                        ,L.DataLeitura AS DataLeitura
                        ,CV.IdComunicadoVersao AS IdComunicadoVersao
                    FROM { _context.PrefixCMS }[Comunicado] AS C
                    INNER JOIN { _context.PrefixCMS }[ComunicadoVersao] AS CV ON C.IdComunicado = CV.IdComunicado AND CV.Versao = (SELECT MAX(CMAX.Versao) FROM { _context.PrefixCMS }[ComunicadoVersao] AS CMAX WHERE CMAX.IdComunicado = C.IdComunicado) 
                    LEFT JOIN { _context.PrefixCMS }[Indicador] AS T1 ON CV.IdTemplate = T1.IdIndicador
                    LEFT JOIN { _context.PrefixCMS }[Tema] AS T2 ON CV.IdTema = T2.IdTema
                    LEFT JOIN { _context.PrefixCMS }[ComunicadoVersaoLog] AS L ON CV.IdComunicadoVersao = L.IdComunicadoVersao AND L.Login = '{login}'
                    WHERE {whereContexto} C.Ativo = 1 AND CV.Ativo = 1 AND CV.Aprovado = 1
                    ORDER BY C.DataPublicacao DESC";

                    connection.Open();

                    var documetos = connection.Query<ComunicadoQueryResult>(sqlDocumentos);

                    return documetos;
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool SalvarComunicado(Comunicado comunicado)
        {
            try
            {
                var entity = _context.Comunicado.FirstOrDefault(item => item.IdComunicado == comunicado.IdComunicado);

                if(entity != null)
                {
                    entity.Editar(
                        comunicado.DataPublicacao,
                        comunicado.DestinatarioDex,
                        comunicado.DestinatarioDexCoordenador,
                        comunicado.DestinatarioUnidade,
                        comunicado.DestinatarioUnidadeCaf,
                        comunicado.DestinatarioUnidadeCoordenadorSest,
                        comunicado.DestinatarioUnidadeCoordenadorSenat,
                        comunicado.DestinatarioUnidadeSupervisor,
                        comunicado.DestinatarioUnidadeDiretor
                        );
                }
                else
                {
                    _context.Comunicado.Add(comunicado);
                }

                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Inscricao: " + ex.Message, ex);
            }
        }

        public bool SalvarLog(ComunicadoVersaoLog comunicadoVersaoLog)
        {
            try
            {
                var retorno = _context.ComunicadoVersaoLog.Add(comunicadoVersaoLog);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao inserir Inscricao: " + ex.Message, ex);
            }
        }

        public bool SalvarVersaoComunicado(ComunicadoVersao comunicadoVersao)
        {

            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string idTema = comunicadoVersao.IdTema == Guid.Empty ? "NULL" : "'" + comunicadoVersao.IdTema.Value.ToString() + "'";
                    
                    var sql = $@"INSERT INTO { _context.PrefixCMS }[ComunicadoVersao]
                                       ([IdComunicadoVersao]
                                       ,[IdComunicado]
                                       ,[IdTemplate]
                                       ,[IdTema]
                                       ,[Versao]
                                       ,[Assunto]
                                       ,[Conteudo]
                                       ,[Ativo]
                                       ,[Aprovado]
                                       ,[DataCadastro]
                                       ,[CriadoPor])
                                 VALUES
                                       ('{comunicadoVersao.IdComunicadoVersao}'
                                       ,'{comunicadoVersao.IdComunicado}'
                                       ,'{comunicadoVersao.IdTemplate}'
                                       ,{ idTema }
                                       ,{comunicadoVersao.Versao}
                                       ,'{comunicadoVersao.Assunto}'
                                       ,'{comunicadoVersao.Conteudo}'
                                       ,1
                                       ,{(comunicadoVersao.Aprovado.Value ? 1 : 0)}
                                       ,GETDATE()
                                       ,'{comunicadoVersao.CriadoPor}')";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool DesativarComunicado(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"UPDATE { _context.PrefixCMS }Comunicado SET Ativo = 0 WHERE IdComunicado = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool AprovarComunicado(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"UPDATE { _context.PrefixCMS }ComunicadoVersao SET Aprovado = 1 WHERE IdComunicadoVersao = '{ id }'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
