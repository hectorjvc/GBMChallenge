using System.Runtime.Serialization;

namespace GeoPosition.API.Application.Dtos
{
    /// <summary>
    /// Objeto para envío de información del Vehpiculo
    /// </summary>
    [DataContract]
    public class VehiculoDto
    {
        /// <summary>
        /// Identificador del Vehículo
        /// </summary>
        [DataMember]
        public int IdVehiculo { get; set; }

        ///Alias del Vehículo
        [DataMember]
        public string AliasVehiculo { get; set; }

        /// <summary> 
        /// Longitud del vehpiculo
        /// </summary>
        [DataMember]
        public double Longitud { get; set; }

        /// <summary>
        /// Latitud del vehículo
        /// </summary>
        [DataMember]
        public double Latitud { get; set; }

      
    }
}
