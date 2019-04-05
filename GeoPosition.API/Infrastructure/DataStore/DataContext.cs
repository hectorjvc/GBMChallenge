using GeoPosition.API.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPosition.API.Infrastructure.DataStore
{
    /// <summary>
    /// Contexto de Datos
    /// </summary>
    public class DataContext : DbContext
    {
        /// <summary>
        /// Constructor, inicializa.
        /// </summary>
        /// <param name="options"></param>
        public DataContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Propiedades de tipo DbSet para Entidad Vehículo 
        /// </summary>
        public virtual DbSet<Vehiculo> Vehiculo { get; set; }

        /// <summary>
        /// Propiedades de tipo DbSet para Entidad LocalizacionHistorica
        /// </summary>
        public virtual DbSet<LocalizacionHistorica> LocalizacionHistorica { get; set; }

        /// <summary>
        /// Validación del objeto DbContextOptionsBuilder
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                throw new NullReferenceException("Configuracion de acceso a datos no encontrada");
        }

        /// <summary>
        /// Asignacion de propiedades a las entidades. 
        /// </summary>
        /// <param name="modelBuilder">Objeto ModelBuilder</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehiculo>(entity => { entity.HasKey(e => e.IdVehiculo); });
            modelBuilder.Entity<LocalizacionHistorica>(entity => { entity.HasKey(e => e.IdLocalizacion); });
        }
    }
}
