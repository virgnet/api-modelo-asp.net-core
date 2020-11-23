using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Commands;
using Shared.FluentValidator;
using System.Data.SqlClient;
using System.Linq;
using WebApi.Command;
using WebApi.QueryResult;

namespace WebApi.Controllers
{
    public class DataBaseController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        [Route("database/select")]
        public ICommandResult Select([FromBody]SelectCommand cmd)
        {
            try
            {
                connectionString = GetConexao(Request.Host.Host);
                using (SqlConnection db = new SqlConnection(connectionString))
                {
                    db.Open();

                    var retorno = new SelectQueryResult();

                    if (cmd.Total)
                    {
                        string queryTotal = "";
                        int z = cmd.Query.ToUpper().IndexOf("ORDER") - cmd.Query.ToUpper().IndexOf("FROM");
                        if (cmd.Query.ToUpper().Contains("ORDER"))
                            queryTotal = "SELECT COUNT(*)" + cmd.Query.Substring(cmd.Query.ToUpper().IndexOf(" FROM "), z);
                        else
                            queryTotal = "SELECT COUNT(*)" + cmd.Query.Substring(cmd.Query.ToUpper().IndexOf(" FROM "));
                        retorno.Total = db.Query<int>(queryTotal).FirstOrDefault();
                    }

                    retorno.Itens = db.Query(cmd.Query).ToList();

                    db.Close();

                    return new CommandResult(true, "Consulta realizada com sucesso.", new { retorno });
                }
            }
            catch (System.Exception)
            {
                return new CommandResult(false, "Dados inválidos", new Notification("DataBase", "Erro ao pesquisar na base"));
            }
        }
    }
}