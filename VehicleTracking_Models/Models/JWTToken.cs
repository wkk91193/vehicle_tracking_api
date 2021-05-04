using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTracking_Models.Models
{
    public class JWTToken
    {
        /// <summary>
        /// JWT Token value
        /// </summary>
        /// <example>JWT_Token</example>
        public string token { get; set; }

        /// <summary>
        /// Expiry date of JWT token
        /// </summary>
        /// <example>2021-05-17T17:09:09Z</example>
        public DateTime expires { get; set; }
    }
}
