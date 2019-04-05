using GeoPosition.API.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoPosition.API.Contracts
{
    /// <summary>
    /// Interfaz de Negocio del Servicio de Vehículo
    /// </summary>
    public interface IVehiculoBusinessServices
    {
        /// <summary>
        /// Agrega un vehículo
        /// </summary>
        /// <param name="vehiculo">Entidad Vehículo</param>
        /// <returns></returns>
        Task<Vehiculo> Add(Vehiculo vehiculo);

        /// <summary>
        /// Elimina un vehículo
        /// </summary>
        /// <param name="vehiculo">Entidad Vehículo</param>
        /// <returns></returns>
        Task<Vehiculo> Remove(Vehiculo vehiculo);
         
        /// <summary>
        /// Actualiza un vehículo
        /// </summary>
        /// <param name="vehiculo">Entidad Vehículo</param>
        /// <returns></returns>
        Task<Vehiculo> Update(Vehiculo vehiculo);

        /// <summary>
        /// Obtiene todos los vehículos disponibles
        /// </summary>
        /// <returns></returns>
        Task<IList<Vehiculo>> ObtenerTodos();

        /// <summary>
        /// Obtiene un vehículo por Identificador
        /// </summary>
        /// <param name="id">Identificador del Vehículo</param>
        /// <returns></returns>
        Task<Vehiculo> ObtenerPorId(int id);
    }
}
