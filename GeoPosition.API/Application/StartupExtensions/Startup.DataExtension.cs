using GeoPosition.API.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GeoPosition.API.Application.StartupExtensions
{
    /// <summary>
    /// Métodos de extención. Objetivo es dejar Startup.cs lo mas ligible posible
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// Registra el contexto de datos.
        /// </summary>
        /// <param name="services">Servicios</param>
        /// <param name="configuration">Configuracion</param>
        /// <returns></returns>
        public static IServiceCollection RegistrarDataContext(this IServiceCollection services,
            IConfiguration configuration) 
        {
            if (configuration == null)
                throw new Exception("configuration is null");

            var connectionString = configuration.GetConnectionString("GBMDatabase");

            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<DataContext>(options => { options.UseSqlServer(connectionString); });

            return services;
        }
    }
}
