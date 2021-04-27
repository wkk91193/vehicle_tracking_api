using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Entities
{
    public class VehicleInformation : BaseEntity
    {
        [JsonPropertyName("vehicleReg")]
        public string vehicleReg { get; set; }

        public List<VehicleLocation> locations { get; set; }


    }
}
