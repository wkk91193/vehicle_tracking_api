using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Entities
{
    public class VehicleInformationEntity : BaseEntity
    {
        [JsonPropertyName("vehicleReg")]
        public string VehicleReg { get; set; }

        [JsonPropertyName("locations")]
        public List<VehicleLocationEntity> Locations { get; set; }


    }
}
