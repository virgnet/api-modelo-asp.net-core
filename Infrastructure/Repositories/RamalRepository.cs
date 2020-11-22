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
    public class RamalRepository : IRamalRepository
    {
        private readonly PortalDoColaboradorDataContext _context;

        public RamalRepository(PortalDoColaboradorDataContext context)
        {
            _context = context;
        }

        public RamalQueryResult DetalheRamal(string login)
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@"SELECT F.Nome, 
                                    F.Login AS Login,
                                    F.Login + '@sestsenat.org.br' AS 'Email', 
                                    T.Numero as 'Telefone',
		                            CASE 
			                        WHEN (C.Nome) IS NULL THEN U.NomeFantasia 
			                        WHEN (C.Nome) IS NOT NULL THEN U.NomeFantasia + ' - ' + C.Nome 
		                            END AS 'Unidade',
		                            CG.Nome AS Cargo, 
		                            A.Bytes AS Foto
                            FROM {_context.PrefixGestaoV1}[Funcionario] F
                            INNER JOIN {_context.PrefixGestaoV1}[TelefoneMeio] TM ON TM.IdFuncionario = F.IdFuncinario
                            INNER JOIN {_context.PrefixGestaoV1}[Telefone] T ON TM.IdTelefone = T.IdTelefone
                            LEFT JOIN {_context.PrefixGestaoV1}[Coordenacao] C ON C.IdCoordenacao = F.IdCoordenacao
                            LEFT JOIN {_context.PrefixGestaoV1}[Unidade] U ON U.IdUnidade = F.IdUnidade
                            LEFT JOIN {_context.PrefixGestaoV1}[Cargo] CG ON CG.IdCargo = F.IdCargo
                            LEFT JOIN {_context.PrefixGestaoV1}[Arquivo] A ON A.IdArquivo = F.IdArquivo
                            where F.Ativo = 1 and F.Login = '{login}'";

                    connection.Open();
                    return  connection.Query<RamalQueryResult>(sql).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        public IEnumerable<RamalQueryResult> ListarRamais()
        {
            using (SqlConnection connection = new SqlConnection(_context.ConnectionString))
            {
                try
                {
                    var sql = $@" SELECT F.Nome, 
                                    F.Login as Login,
                                    F.Login + '@sestsenat.org.br' AS 'Email', 
                                    T.Numero as 'Telefone',
		                            CASE 
			                        WHEN (C.Nome) IS NULL THEN U.NomeFantasia 
			                        WHEN (C.Nome) IS NOT NULL THEN U.NomeFantasia + ' - ' + C.Nome 
		                            END AS 'Unidade',
		                            CG.Nome AS Cargo
                            FROM {_context.PrefixGestaoV1}[Funcionario] F
                            INNER JOIN {_context.PrefixGestaoV1}[TelefoneMeio] TM ON TM.IdFuncionario = F.IdFuncinario
                            INNER JOIN {_context.PrefixGestaoV1}[Telefone] T ON TM.IdTelefone = T.IdTelefone
                            LEFT JOIN {_context.PrefixGestaoV1}[Coordenacao] C ON C.IdCoordenacao = F.IdCoordenacao
                            LEFT JOIN {_context.PrefixGestaoV1}[Unidade] U ON U.IdUnidade = F.IdUnidade
                            LEFT JOIN {_context.PrefixGestaoV1}[Cargo] CG ON CG.IdCargo = F.IdCargo
                            where F.Ativo = 1 AND F.Nome IS NOT NULL
                            ORDER BY 1";

                    connection.Open();
                    return connection.Query<RamalQueryResult>(sql);
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
