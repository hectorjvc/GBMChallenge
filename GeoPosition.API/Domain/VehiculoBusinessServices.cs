using GeoPosition.API.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPosition.API.Domain
{
    /// <summary>
    /// Entidad de Negocio del Vehículo, implementa la interfaz IVehiculoBusinessServices
    /// </summary>
    public class VehiculoBusinessServices : IVehiculoBusinessServices
    {

        /// <summary>
        /// Miembro privado para acceder al servicio.
        /// Siempre basado en la Interfaz
        /// </summary>
        private readonly IVehiculoRepository _vehiculoRepository;

        /// <summary>
        /// Constructor, inicializa el servicio
        /// </summary> 
        /// <param name="vehiculoRepository"></param>
        public VehiculoBusinessServices(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        /// <summary>
        /// Agrega un nuevo vehículo, valida la Idempotencia
        /// </summary>
        /// <param name="vehiculo">entidad vehículo</param>
        /// <returns></returns>
        public async Task<Vehiculo> Add(Vehiculo vehiculo)
        {
            // product has been added.
            var targetVehicle = await _vehiculoRepository.GetByIdWithIdempotencyCheck(vehiculo.IdVehiculo);
            if (targetVehicle == null)
            {
                await _vehiculoRepository.Add(vehiculo);
                AlmacenaHistorial(vehiculo);
            }
            else
            {
                // Assign Id from original insert
                vehiculo.IdVehiculo = targetVehicle.IdVehiculo;
            }

            
            return vehiculo;
        }

        /// <summary>
        /// Almacena el histórico
        /// </summary>
        /// <param name="vehiculo"></param>
        protected async void AlmacenaHistorial(Vehiculo vehiculo)
        {
            await _vehiculoRepository.RegistraHistorial(new LocalizacionHistorica
            {
                IdVehiculo = vehiculo.IdVehiculo,
                AliasVehiculo = vehiculo.AliasVehiculo,
                FechaRegistro = DateTime.Now,
                Latitud = vehiculo.Latitud,
                Longitud = vehiculo.Longitud
            });
        }

        /// <summary>
        /// Obtiene un vehículo por Identifiacor
        /// </summary>
        /// <param name="id">Identificador del vehículo</param>
        /// <returns></returns>
        public async Task<Vehiculo> ObtenerPorId(int id)
        {
            return await _vehiculoRepository.ObtenerPorId(id);
        }

        /// <summary>
        /// Obtiene todos los vehículos almacenados
        /// </summary>
        /// <returns></returns>
        public async Task<IList<Vehiculo>> ObtenerTodos()
        {
            return await _vehiculoRepository.ObtenerTodos();
        }

        /// <summary>
        /// Remueve un vehículo
        /// </summary>
        /// <param name="vehiculo">entidad vehículo</param>
        /// <returns></returns>
        public Task<Vehiculo> Remove( Vehiculo vehiculo)
        {
            throw new NotImplementedException();
            //TODO: Implementar el borrado
        }

        /// <summary>
        /// Actualiza un vehículo
        /// </summary>
        /// <param name="vehiculo">entidad vehículo</param>
        /// <returns></returns>
        public async Task<Vehiculo> Update( Vehiculo vehiculo)
        {
            await _vehiculoRepository.Update(vehiculo);
            AlmacenaHistorial(vehiculo);
            return vehiculo;
        }
    }
}
