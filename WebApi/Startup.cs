using Domain.Handlers;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.DataContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace WebApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            {
                options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });

            //DEPENDENCY INJECTION
            services.AddScoped<ModeloDataContext, ModeloDataContext>();
            services.AddTransient<IPublicacaoRepository, PublicacaoRepository>();
            services.AddTransient<ITemaRepository, TemaRepository>();
            services.AddTransient<IPublicacaoTemaRepository, PublicacaoTemaRepository>();
            services.AddTransient<IAutenticacaoRepository, AutenticacaoRepository>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<PublicacaoHandler, PublicacaoHandler>();
            services.AddTransient<TemaHandler, TemaHandler>();

            services.AddSwaggerGen(x =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);

                //SWAGGER
                x.SwaggerDoc("v1", new Info { Title = Configuration["ApplicationSettings:Name"], Version = Configuration["ApplicationSettings:Version"] });
            });

            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "10.3.0.84:6379";
            });

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyMethod().AllowAnyOrigin().AllowCredentials().AllowAnyHeader());
            });

            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var swagger = "/swagger/v1/swagger.json";

            if (bool.Parse(Configuration["ApplicationSettings:Debug"]))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                swagger = "/colaborador-api/swagger/v1/swagger.json";
                //swagger = "/portalcolaboradorapi/swagger/v1/swagger.json";
                app.UseHsts();
            }

            // Habilitar CORS 
            app.UseCors("CorsPolicy");

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swagger, Configuration["ApplicationSettings:Name"] + " - " + Configuration["ApplicationSettings:Version"]);
            });

            app.UseMvc();

            
        }
    }
}
