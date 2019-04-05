using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using RestCommunication;
using ServiceDiscovery;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace ApiGateway.API
{
    public class Startup
    {
        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
         
        /// <summary>
        /// Miembro privado para inyección de dependencias
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(
                config =>
                {
                    config.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                    config.InputFormatters.Add(new XmlSerializerInputFormatter());
                    config.RespectBrowserAcceptHeader = true;
                });

            // Add Swagger (OpenId Connect) plumbing
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "API Gateway Services",
                    Version = "v1",
                    Description = "Encapsulación de la funcionalidad detrás de esta API."
                });

                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "ApiGateway.API.xml");
                c.IncludeXmlComments(xmlPath);
            });

            // Plubming class for ServiceLocator
            services.AddSingleton<IServiceLocator, ServiceLocator>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Switch to enable Resilient RestClient component
            if (Configuration.GetValue<string>("UseResilientRestClient") == bool.TrueString)
            {
                services.AddSingleton<IRestClient, ResilientRestClient>();
            }
            else
            {
                // User does not want to use Resilient RestClient library
                services.AddScoped<IRestClient, RestClient>();
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Gateway Services V1"); });

        }
    }
}
