using GeoPosition.API.Contracts;
using GeoPosition.API.Domain;
using GeoPosition.API.Infrastructure.DataStore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace GeoPosition.API.Infrastructure.Repository
{
    /// <summary>
    /// Repositorio Vehículo, hereda del repositorio base BaseRepository
    /// </summary>
    public class VehiculoRepository : BaseRepository<Vehiculo>, IVehiculoRepository
    {

        
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="ctx"></param>
        public VehiculoRepository(DataContext ctx) : base(ctx)
        {
            
        }

        /// <summary>
        /// Obtiene por Id
        /// </summary>
        /// <param name="id">Identificador del Vehículo</param>
        /// <returns></returns>
        public async Task<Vehiculo> ObtenerPorId(int id)
        {
            return FindById(id).Result;
        }

        /// <summary>
        /// Obtiene todos los vehículos
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Vehiculo>> ObtenerTodos()
        {
            return  Get().ToList();
        }

        /// <summary>
        /// Valida la Idempotencia
        /// </summary>
        /// <param name="id">Identificador del Vehículo</param>
        /// <returns></returns>
        public async Task<Vehiculo> GetByIdWithIdempotencyCheck(int id)
        {
            return Get().Where(x => x.IdVehiculo == id).FirstOrDefault();
        }
    }
}
