using GeoPosition.API.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPosition.API.Infrastructure.DataStore
{
    /// <summary>
    /// Inicializador de la base de datos. 
    /// Puede esta un método Seed(), no implementado
    /// </summary>
    public class DataInitializer
    {
        /// <summary>
        /// Valida que esté creada la base de datos.
        /// </summary>
        /// <param name="serviceScope"></param>
        public static void InitializeDatabaseAsync(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetService<DataContext>(); 

            if (context != null)
            {
                var databaseCreated = context.Database.EnsureCreated();
            }
        }        
    }
}