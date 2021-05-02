using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking_Models.Models
{
    public class VehicleInformationModel
    {
     
        public string VehicleReg { get; set; }

        public List<VehicleLocationModel> VehicleLocations { get; set; }

       

    }
}
