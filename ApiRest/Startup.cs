using Domain.Handlers;
using Domain.Repositories;
using Domain.Services;
using Infrastructure.DataContext;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Shared.Commands;
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
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1).AddJsonOptions(options =>
            //{
            //    options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            //});

            //DEPENDENCY INJECTION
            services.AddScoped<ModeloDataContext, ModeloDataContext>();
            services.AddTransient<IPublicacaoRepository, PublicacaoRepository>();
            services.AddTransient<ITemaRepository, TemaRepository>();
            services.AddTransient<IPublicacaoTemaRepository, PublicacaoTemaRepository>();
            services.AddTransient<IAutenticacaoRepository, AutenticacaoRepository>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<PublicacaoHandler, PublicacaoHandler>();
            services.AddTransient<TemaHandler, TemaHandler>();


            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();
            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("TokenConfigurations"))
                    .Configure(tokenConfigurations);
            services.AddSingleton(tokenConfigurations);


            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });


            services.AddSwaggerGen(x =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                x.IncludeXmlComments(xmlPath);

                //SWAGGER
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = Configuration["ApplicationSettings:Name"], Version = Configuration["ApplicationSettings:Version"] });
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
