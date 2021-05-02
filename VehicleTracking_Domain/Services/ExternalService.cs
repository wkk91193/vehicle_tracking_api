using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VehicleTracking_Domain.Helpers;
using VehicleTracking_Domain.Services.Interfaces;

namespace VehicleTracking_Domain.Services
{
    public class ExternalService : IExternalService
    {
        private readonly IConfiguration _configuration;
        public ExternalService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<string> GetAddressFromCoordinates(double latitude, double longitude)
        {
            string coordinates = latitude + "," + longitude;
            var response = await Http.GetHttpClientInstance().GetAsync(new Uri(this._configuration["GoogleMapsGeoCodeAPI:URL"] + "?latlng=" + coordinates + "&key=" + this._configuration["GoogleMapsGeoCodeAPI:ApiKey"]));
            dynamic content = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());
            var formattedAddress = content.results[0].formatted_address;
            return formattedAddress;
        }
    }
}
