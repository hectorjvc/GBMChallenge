 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeoPosition.API.Domain
{
    /// <summary>
    /// Para almacenar la localización histórica
    /// </summary>
    public class LocalizacionHistorica
    {
        /// <summary>
        /// Identifiador propio de la entidad
        /// </summary>
        public int IdLocalizacion { get; set; }
        
        /// <summary>
        /// Identificador del vehículo
        /// </summary>
        public int IdVehiculo { get; set; }

        /// <summary>
        /// Alias del vehículo
        /// </summary>
        public string AliasVehiculo { get; set; }

        /// <summary>
        /// Longitud del vehículo
        /// </summary>
        public double Longitud { get; set; }

        /// <summary>
        /// Latitud del vehículo
        /// </summary>
        public double Latitud { get; set; }

        /// <summary>
        /// Fecha de registro del vehículo en el Historial
        /// </summary>
        public DateTime FechaRegistro { get; set; }

    }
}
