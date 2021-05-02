using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking_Models.Models
{
    public class RecordPositionModel:ParentModel
    {
        public string UserName { get; set; }
        public string VehicleReg { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timestamp { get; set; }
    }
}
