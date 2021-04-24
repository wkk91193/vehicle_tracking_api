using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Entities
{
    public class VehicleUser : BaseEntity
    {
        [JsonPropertyName("firstName")]
        public string firstName { get; set; }

        [JsonPropertyName("lastName")]
        public string lastName { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("roleType")]
        public string roleType { get; set; }

        [JsonPropertyName("vehicleInfo")]
        public List<VehiclePosition> vehicleInfo { get; set; }
      
    }
}
