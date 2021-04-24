using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Entities
{
    public class VehiclePosition : BaseEntity
    {
        [JsonPropertyName("vehicleReg")]
        public string vehicleReg { get; set; }

        [JsonPropertyName("currentPosition")]
        public string currentPosition { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime timestamp { get; set; }

    }
}
