using GeoPosition.API.Application.Dtos;
using GeoPosition.API.Contracts;
using GeoPosition.API.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;

namespace GeoPosition.API.Controllers
{
    /// <summary>
    /// Controlador de los Vehículos
    /// </summary>
    [Route("api/[controller]")]
    
    public class VehicleController : Controller
    {
        /// <summary>
        /// Para acceso al servicio de negocio del repositorio de vehículos
        /// </summary>
        private readonly IVehiculoBusinessServices _vehicleBusinessServices;

        /// <summary>
        /// Constructor, inicializa el servicio del repositorio de vehículos
        /// </summary>
        /// <param name="vehicleBusinessServices"></param>
        public VehicleController(IVehiculoBusinessServices vehicleBusinessServices)
        {
            _vehicleBusinessServices = vehicleBusinessServices;
        }

        /// <summary>
        /// Obtiene todos los vehículos
        /// </summary>
        /// <returns>Un objeto Vehículo</returns>
        [ProducesResponseType(typeof(List<Vehiculo>), 200)]
        [HttpGet("Vehiculos", Name = "ObtenerTodos")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var vehiculos = await _vehicleBusinessServices.ObtenerTodos();
      
            if (vehiculos.Count < 1)
                return new ObjectResult(new List<VehiculoDto>());

            //TODO: Realizar mapeo
            // ObjectResult return type is capable of content negotiation
            return new ObjectResult((vehiculos)); //Mapper.MapToMusicDto
        }

        /// <summary>
        /// Consulta la posicion de un vehículo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(VehiculoDto), 200)]
        [HttpGet("Vehiculos/ConsultaPosicion/{id}", Name = "ConsultaPosicion")]
        public async Task<IActionResult> ConsultaPosicion(int id)
        {
            VehiculoDto v = null; 
            var vehiculo = await _vehicleBusinessServices.ObtenerPorId(id);
            
            if (vehiculo != null)
            {
                v = new VehiculoDto
                {
                    IdVehiculo = vehiculo.IdVehiculo,
                    AliasVehiculo = vehiculo.AliasVehiculo,
                    Longitud = vehiculo.Longitud,
                    Latitud = vehiculo.Latitud
                };
            }
            else if (vehiculo == null)
                return new ObjectResult(v);

            //TODO: Realizar mapeo
            // ObjectResult return type is capable of content negotiation
            return new ObjectResult(v); //Mapper.MapToMusicDto
        }

        /// <summary>
        /// Almacena un nuevo vehículo
        /// </summary>
        /// <param name="vehiculoDto"></param>
        /// <returns>El vehículo nuevo</returns>
        [ProducesResponseType(typeof(VehiculoDto), 201)]
        [HttpPost("Vehiculos/AlmacenaVehiculo", Name = "AlmacenaVehiculo")]
        public async Task<IActionResult> AlmacenaVehiculo([FromBody] VehiculoDto vehiculoDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Guard.ForNullObject(vehiculoDto, "La clase vehiculoDto falta");

            Vehiculo nuevoVehiculo = new Vehiculo
            {
                IdVehiculo = vehiculoDto.IdVehiculo,
                AliasVehiculo = vehiculoDto.AliasVehiculo,
                Latitud = vehiculoDto.Latitud,
                Longitud = vehiculoDto.Longitud
            };

            await _vehicleBusinessServices.Add(nuevoVehiculo);

            var v = new VehiculoDto
            {
                IdVehiculo = nuevoVehiculo.IdVehiculo,
                AliasVehiculo = nuevoVehiculo.AliasVehiculo,
                Latitud = nuevoVehiculo.Latitud, 
                Longitud = nuevoVehiculo.Longitud
            };

            return new ObjectResult(v);
        }
    }
}