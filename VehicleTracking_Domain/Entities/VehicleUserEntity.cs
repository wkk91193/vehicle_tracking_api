using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Entities
{
    public class VehicleUserEntity : BaseEntity
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("roleType")]
        public string RoleType { get; set; }

        [JsonPropertyName("vehicleInfo")]
        public VehicleInformationEntity VehicleInfo { get; set; }
      
    }
}
