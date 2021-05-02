using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VehicleTracking_Domain.Services.Interfaces
{
    public interface IExternalService
    {
        public Task<string> GetAddressFromCoordinates(double latitude, double longitude);
    }
}
