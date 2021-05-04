using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking_Models.Models
{
    public class VehicleInformationModel
    {
        /// <summary>
        ///  Vehicle Registration Number
        /// </summary>
        /// <example>DF-3461</example>
        public string VehicleReg { get; set; }

        public List<VehicleLocationModel> VehicleLocations { get; set; }

       

    }
}
