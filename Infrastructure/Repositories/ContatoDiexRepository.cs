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
    public class ContatoDiexRepository : IContatoDiexRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public ContatoDiexRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public IEnumerable<AssuntoContatoDiexQueryResult> ListarAssuntoContatoDiex()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var sql = $@"SELECT IdIndicador AS IdAssunto, Titulo FROM { _context.PrefixCMS }[Indicador] WHERE IdTipoIndicador = 'EF0555B8-6756-421B-A9FF-EBEC004086EF' ORDER BY 2";
                    return connection.Query<AssuntoContatoDiexQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public ContatoDiexQueryResult BuscarContatoDiex(Guid id)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sqlDocumentos = $@"SELECT C.[IdContatoDiex]
                        ,C.[IdAssunto]
	                    ,I.[Titulo] AS Assunto
                        ,C.[IdSituacao]
	                    ,S.[Titulo] AS Situacao
                        ,C.[LoginRemetente]
                        ,C.[Mensagem]
                        ,C.[DataMensagem]
                        ,C.[LoginResposta]
                        ,C.[Resposta]
                        ,C.[DataResposta]
                        ,C.[Ativo]
                        ,C.[Area]
                        ,C.[Nome]
                        ,C.[Origem]
                    FROM { _context.PrefixCMS }[ContatoDiex] AS C
                    INNER JOIN { _context.PrefixCMS }[Indicador] AS S ON C.IdSituacao = S.IdIndicador
                    INNER JOIN { _context.PrefixCMS }[Indicador] AS I ON C.IdAssunto = I.IdIndicador
                    WHERE C.IdContatoDiex = '{ id }'";

                    connection.Open();

                    var contato = connection.Query<ContatoDiexQueryResult>(sqlDocumentos).FirstOrDefault();

                    return contato;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<ContatoDiexQueryResult> PesquisarContatoDiex(Guid? idAssunto, Guid? idSituacao, string textoDaBusca)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    string whereAssunto = "";
                    if (idAssunto.HasValue)
                        whereAssunto = $@" C.IdAssunto = '{ idAssunto }' AND ";

                    string whereSituacao = "";
                    if (idSituacao.HasValue)
                        whereSituacao = $@" C.IdSituacao = '{ idSituacao }' AND ";

                    var sqlDocumentos = $@"SELECT C.[IdContatoDiex]
                        ,C.[IdAssunto]
	                    ,I.[Titulo] AS Assunto
                        ,C.[IdSituacao]
	                    ,S.[Titulo] AS Situacao
                        ,C.[LoginRemetente]
                        ,C.[Mensagem]
                        ,C.[DataMensagem]
                        ,C.[LoginResposta]
                        ,C.[Resposta]
                        ,C.[DataResposta]
                        ,C.[Ativo]
                        ,C.[Area]
                        ,C.[Nome]
                        ,C.[Origem]
                    FROM { _context.PrefixCMS }[ContatoDiex] AS C
                    INNER JOIN { _context.PrefixCMS }[Indicador] AS I ON C.IdAssunto = I.IdIndicador
                    INNER JOIN { _context.PrefixCMS }[Indicador] AS S ON C.IdSituacao = S.IdIndicador
                    WHERE C.Ativo = 1 AND {whereAssunto} {whereSituacao} (C.Mensagem LIKE '%{ textoDaBusca }%' OR C.Resposta LIKE '%{ textoDaBusca }%')
                    ORDER BY C.[DataMensagem] DESC";

                    connection.Open();

                    var contato = connection.Query<ContatoDiexQueryResult>(sqlDocumentos);

                    return contato;
                }
                catch
                {
                    return null;
                }
            }
        }

        public bool SalvarMensagemContatoDiex(ContatoDiex contatoDiex)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"INSERT INTO { _context.PrefixCMS }[ContatoDiex]
                                        ([IdContatoDiex]
                                        ,[IdAssunto]
                                        ,[LoginRemetente]
                                        ,[Mensagem]
                                        ,[DataMensagem]
                                        ,[Ativo]
                                        ,[Area]
                                        ,[Nome]
                                        ,[Origem])
                                    VALUES
                                        ('{contatoDiex.IdContatoDiex}'
                                        ,'{contatoDiex.IdAssunto}'
                                        ,'{contatoDiex.LoginRemetente}'
                                        ,'{contatoDiex.Mensagem}'
                                        ,GETDATE()
                                        ,1
                                        ,'{contatoDiex.Area}'
                                        ,'{contatoDiex.Nome}'
                                        ,'{contatoDiex.Origem}')";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool SalvarRespostaContatoDiex(ContatoDiex contatoDiex)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"UPDATE { _context.PrefixCMS }[ContatoDiex]
                                SET [LoginResposta] = '{contatoDiex.LoginResposta}'
                                    ,[Resposta] = '{contatoDiex.Resposta}'
                                    ,[DataResposta] = GETDATE()
                                    ,[IdSituacao] = '0FABC950-CFF0-4DA1-BA5E-7A941DD003F2'

                                WHERE IdContatoDiex = '{contatoDiex.IdContatoDiex}'";
                    connection.Open();
                    return connection.Query<bool>(sql).FirstOrDefault();
                }
                catch
                {
                    return false;
                }
            }
        }

        public IEnumerable<SituacaoContatoDiexQueryResult> ListarSituacaoContatoDiex()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    connection.Open();
                    var sql = $@"SELECT IdIndicador AS IdSituacao, Titulo FROM { _context.PrefixCMS }[Indicador] WHERE IdTipoIndicador = 'B8BD8027-1187-4A60-8745-EC2629DD34E0' ORDER BY 2";
                    return connection.Query<SituacaoContatoDiexQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public bool LerContatoDiex(Guid idContatoDiex)
        {
            try
            {
                var obj = _context.ContatoDiex.Where(c => c.IdContatoDiex.Equals(idContatoDiex)).FirstOrDefault();
                if (obj != null)
                {
                    obj.LerMensagem();
                    _context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
