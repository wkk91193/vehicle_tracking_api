using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking_Models.Models
{
    public class VehicleLocationModel
    {
        /// <summary>
        ///  Latitude of current GPS coordinate
        ///  Ex: 13.67811
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        ///    Longitude of current GPS coordinate
        /// </summary>
        /// <example>100.629204</example>
        public double Longitude { get; set; }

        /// <summary>
        ///    Timestamp at the time sending GPS position
        /// </summary>
        /// <example>2020-07-25 07:36:51.239</example>
        public string Timestamp { get; set; }

        /// <summary>
        ///    Address of current GPS coordinate using Google Maps API
        /// </summary>
        /// <example>122 Sukhumvit 103 Rd, Khwaeng Bang Na, Khet Bang Na, Krung Thep Maha Nakhon 10260, Thailand</example>
        public string AddressOfLocation { get; set; }
    }
}
