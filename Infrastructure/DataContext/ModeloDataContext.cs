using Domain.Entities;
using Infrastructure.ConfigurationMap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.FluentValidator;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Infrastructure.DataContext
{
    public class ModeloDataContext : DbContext
    {
        public static IConfiguration Configuration { get; set; }
        public SqlConnection Connection { get; set; }
        public string ConnectionString { get; set; }

        public DbSet<Acesso> Acesso { get; set; }
        public DbSet<Publicacao> Publicacao { get; set; }
        public DbSet<Tema> Tema { get; set; }
        public DbSet<PublicacaoTema> PublicacaoTema { get; set; }

        
        //Utilizado somente no update-database
        public ModeloDataContext()
        {
            //Alterar para o ambiente desejado no momento de criação e atualização do banco de dados
            string ambiente = "Production";
            //string ambiente = "Development";
            //string ambiente = "Staging";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"appsettings.{ambiente}.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        //Construtor para debug e ambientes publicados
        public ModeloDataContext(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.Production.json", optional: true);

            Configuration = builder.Build();

            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Notification>();

            builder.ApplyConfiguration(new PublicacaoMap());
            builder.ApplyConfiguration(new TemaMap());
            builder.ApplyConfiguration(new PublicacaoTemaMap());
            builder.ApplyConfiguration(new AcessoMap());

            var cascadeFKs = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(builder);
        }
    }
}