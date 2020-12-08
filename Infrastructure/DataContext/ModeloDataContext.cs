using Domain.Entities;
using Infrastructure.ConfigurationMap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Shared.FluentValidator;
using System;
using System.Data.SqlClient;
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

        public ModeloDataContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

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