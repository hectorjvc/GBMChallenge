using GeoPosition.API.Application.StartupExtensions;
using GeoPosition.API.Contracts;
using GeoPosition.API.Domain;
using GeoPosition.API.Infrastructure.DataStore;
using GeoPosition.API.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

namespace GeoPosition.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
         
        /// <summary>
        /// Miembro para manejar la Configuracion
        /// </summary>
        public IConfiguration Configuration { get; }


        /// <summary>
        /// Configura los servicios disponibles en la aplicacion
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegistrarDataContext(Configuration);
            services.AddScoped<IVehiculoRepository, VehiculoRepository>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IVehiculoBusinessServices, VehiculoBusinessServices>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Geo Position API",
                    Version = "v1",
                    Description = "Expone servicios relacionados a un vehículo"
                });
                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "GeoPosition.API.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Geo Position API V1"); });

            // Ensures Product database is created and fully-populated
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                DataInitializer.InitializeDatabaseAsync(serviceScope);
            }
        }
    }
}
