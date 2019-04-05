using System.Runtime.Serialization;

namespace ApiGateway.API.Dtos
{
    /// <summary>
    /// Objeto vehículo DTO
    /// </summary>
    [DataContract]
    public class VehiculoDto
    {
        /// <summary>
        /// Identificador del Vehiculo
        /// </summary>
        [DataMember]
        public int IdVehiculo { get; set; }

        /// <summary>
        /// Alias del vehiculo
        /// </summary>
        [DataMember]
        public string AliasVehiculo { get; set; }
         
        /// <summary>
        /// Longitude del Vehículo
        /// </summary>
        [DataMember]
        public double Longitud { get; set; }

        /// <summary>
        /// Latitud del Vehículo
        /// </summary>
        [DataMember]
        public double Latitud { get; set; }
       
    }
}
