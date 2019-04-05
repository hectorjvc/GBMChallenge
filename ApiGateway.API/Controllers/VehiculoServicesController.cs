using ApiGateway.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using RestCommunication;
using ServiceDiscovery;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiGateway.API.Controllers
{
    /// <summary>
    /// Controller de GateWay Vehiculo
    /// </summary>
    public class VehiculoServicesController : Controller
    {

        private readonly IRestClient _restClient;

        /// <summary>
        /// Inicializacion del Servicio
        /// </summary> 
        /// <param name="restClient"></param>
        public VehiculoServicesController(IRestClient restClient)
        { 
           _restClient = restClient;
        }

        /// <summary>
        /// Obtiene todos los vehículos
        /// </summary>
        /// <returns>Un objeto Vehículo</returns>
        [ProducesResponseType(typeof(VehiculoDto), 200)]
        [HttpGet("Vehiculos/ObtenerTodos", Name = "ObtenerTodosGatewayRoute")]
        public async Task<IActionResult> ObtenerTodos()
        {
            var result = await _restClient.GetAsync<List<VehiculoDto>>(ServiceEnum.GeoPosition,
                $"api/Vehicle/Vehiculos/");

            if (result == null || result.Count < 1)
                return BadRequest("No hay vehiculos");

            return new ObjectResult(result);
        }

        /// <summary>
        /// Consulta la posicion de un vehículo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [ProducesResponseType(typeof(string), 200)]
        [HttpGet("Vehiculos/ConsultaPosicion/{id}", Name = "ConsultaPosicionGatewayRoute")]
        public async Task<IActionResult> ConsultaPosicion(int id)
        {
            var result = await _restClient.GetAsync<VehiculoDto>(ServiceEnum.GeoPosition,
                $"api/Vehicle/Vehiculos/ConsultaPosicion/{id}");

            if (result == null )
                return BadRequest("No existe el vehiculo");

            //return new ObjectResult(result);
            return new ObjectResult(result);
        }

        /// <summary>
        /// Almacena un nuevo vehículo
        /// </summary>
        /// <param name="vehiculoDto"></param>
        /// <returns>El vehículo nuevo</returns>
        [ProducesResponseType(typeof(VehiculoDto), 201)]
        [HttpPost("Vehiculos/AlmacenaVehiculo", Name = "AlmacenaVehiculoGatewayRoute")]
        public async Task<IActionResult> AlmacenaVehiculo([FromBody] VehiculoDto vehiculoDto)
        {
            
            var response = await _restClient.PostAsync<VehiculoDto>(ServiceEnum.GeoPosition,
               $"api/Vehicle/Vehiculos/AlmacenaVehiculo", vehiculoDto);

            if (response == null )
                return BadRequest("No se pudo almacenar el vehiculo");

            return new ObjectResult(response);
        }

    }
}