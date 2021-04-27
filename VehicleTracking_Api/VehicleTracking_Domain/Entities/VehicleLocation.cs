using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Entities
{
    public class VehicleLocation : BaseEntity
    {
        [JsonPropertyName("latitude")]
        public double latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double longitude { get; set; }

        [JsonPropertyName("timestamp")]
        public string timestamp { get; set; }
    }
}
