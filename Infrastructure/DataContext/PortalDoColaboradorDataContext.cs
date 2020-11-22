using Domain.Entities;
using Infrastructure.ConfigurationMap;
using Infrastructure.Repositories.ConfigurationMap;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.FluentValidator;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Infrastructure.DataContext
{
    public class PortalDoColaboradorDataContext : DbContext
    {
        public static IConfiguration Configuration { get; set; }
        public SqlConnection Connection { get; set; }
        public string ConnectionString { get; set; }
        public string PrefixCMS { get; set; }
        public string PrefixSIGOP { get; set; }
        public string PrefixGestaoV1 { get; set; }
        public string PrefixGestaoV2 { get; set; }
        public string PrefixSIRDOC { get; set; }

        public DbSet<Calendario> Calendario { get; set; }
        public DbSet<Comunicado> Comunicado { get; set; }
        public DbSet<ComunicadoVersao> ComunicadoVersao { get; set; }
        public DbSet<ComunicadoVersaoLog> ComunicadoVersaoLog { get; set; }
        public DbSet<ContatoDiex> ContatoDiex { get; set; }
        public DbSet<Publicacao> Publicacao { get; set; }
        public DbSet<Tema> Tema { get; set; }
        public DbSet<TemaRelacionamento> TemaRelacionamento { get; set; }
        public DbSet<Indicador> Indicador { get; set; }
        public DbSet<TipoIndicador> TipoIndicador { get; set; }

        //Utilizado somente no update-database
        public PortalDoColaboradorDataContext()
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
        public PortalDoColaboradorDataContext(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.Production.json", optional: true);

            Configuration = builder.Build();

            ConnectionString = Configuration["ConnectionStrings:DefaultConnection"];
            PrefixCMS = Configuration["DataBaseSettings:PrefixCMS"];
            PrefixSIGOP = Configuration["DataBaseSettings:PrefixSIGOP"];
            PrefixGestaoV1 = Configuration["DataBaseSettings:PrefixGestaoV1"];
            PrefixGestaoV2 = Configuration["DataBaseSettings:PrefixGestaoV2"];
            PrefixSIRDOC = Configuration["DataBaseSettings:PrefixSIRDOC"];
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Ignore<Notification>();

            builder.ApplyConfiguration(new CalendarioMap());
            builder.ApplyConfiguration(new ComunicadoMap());
            builder.ApplyConfiguration(new ComunicadoVersaoMap());
            builder.ApplyConfiguration(new ComunicadoVersaoLogMap());
            builder.ApplyConfiguration(new ContatoDiexMap());
            builder.ApplyConfiguration(new PublicacaoMap());
            builder.ApplyConfiguration(new TemaMap());
            builder.ApplyConfiguration(new TemaRelacionamentoMap());
            builder.ApplyConfiguration(new IndicadorMap());
            builder.ApplyConfiguration(new TipoIndicadorMap());

            var cascadeFKs = builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;

            base.OnModelCreating(builder);
        }
    }
}