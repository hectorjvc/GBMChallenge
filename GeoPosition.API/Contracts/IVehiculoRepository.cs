using GeoPosition.API.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoPosition.API.Contracts
{

    /// <summary>
    /// Interfaz del repositorio de Vehiculos
    /// </summary>
    public interface IVehiculoRepository : IRepository<Vehiculo>
    {
        /// <summary>
        /// Obtiene todos los vehículos
        /// </summary>
        /// <returns>Lista de vehículos</returns>
        Task<IList<Vehiculo>> ObtenerTodos();

        /// <summary>
        /// Obtiene un vehículo basado en el Identifiacador
        /// </summary>
        /// <param name="id">Identificador del Vehículo</param>
        /// <returns></returns>
        Task<Vehiculo> ObtenerPorId(int id);
         
        /// <summary>
        /// Valida la existencia previa de un vehículo ya sea para actualizarlo o eliminarlo
        /// </summary>
        /// <param name="id">Identificador del Vehículo</param>
        /// <returns></returns>
        Task<Vehiculo> GetByIdWithIdempotencyCheck(int id);
        
    }
}
