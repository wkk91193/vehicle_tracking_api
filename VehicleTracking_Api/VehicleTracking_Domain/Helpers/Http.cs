using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Helpers
{
    // a singleton class
    public sealed class Http
    {


        private static readonly Lazy<HttpClient> lazyClient = new Lazy<HttpClient>(() => new HttpClient());

        // a private constructor
        private Http()
        {
        }

        // the method that returns an instance
        public static HttpClient GetHttpClientInstance()
        {

            return lazyClient.Value;
        }
    }
}
