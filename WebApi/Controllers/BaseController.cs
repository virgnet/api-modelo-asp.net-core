using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

namespace WebApi.Controllers
{
    public class BaseController : ControllerBase
    {
        public string authenticatedUser;
        public string connectionString;
        public string fileServerString;

        public IConfigurationRoot configuration { get; set; }

        public BaseController()
        {
            if (User != null)
            {
                var userAccount = User.Claims.FirstOrDefault(c => c.Type.Contains("identity/claims/name"));
                if (userAccount != null)
                    authenticatedUser = userAccount.Value;
            }
            else
            {
                authenticatedUser = "anônimo";
            }
        }

        public string GetConexao(string host)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            if (configuration.GetValue<string>("Host:Development").Equals(host))
                return configuration.GetConnectionString("DefaultConnection");
            else if (configuration.GetValue<string>("Host:Production").Equals(host))
                return configuration.GetConnectionString("ProductionConnection");
            else
                return "";
        }

    }
}