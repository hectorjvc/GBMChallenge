using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPosition.API.Domain
{
    /// <summary>
    /// Entidad Vehículo
    /// </summary>
    public class Vehiculo
    {
        /// <summary>
        /// Identificador del vehículo
        /// </summary>
        public int IdVehiculo { get; set; }

        /// <summary>
        /// Alias del vehículo
        /// </summary>
        public string AliasVehiculo { get; set; }

        /// <summary> 
        /// Longitud del Vehículo
        /// </summary>
        public double Longitud { get; set; }

        /// <summary>
        /// Latitud del vehículo
        /// </summary>
        public double Latitud { get; set; }
        
    }
}
