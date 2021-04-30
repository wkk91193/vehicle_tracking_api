using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Domain.Repository.Interfaces;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services.Interfaces
{
    public class VehicleLocationService : IVehicleLocationService
    {
        private readonly IVehicleLocationRepository _vehicleLocationRepository;
        private readonly IVehicleUserService _vehicleUserService;

        public VehicleLocationService(IVehicleLocationRepository vehicleLocationRepository,
                                      IVehicleUserService vehicleUserService)
        {
            this._vehicleLocationRepository = vehicleLocationRepository
                                      ?? throw new ArgumentNullException(nameof(vehicleLocationRepository));

            this._vehicleUserService = vehicleUserService
                                     ?? throw new ArgumentNullException(nameof(vehicleUserService));

        }

        public async Task<bool> RecordPosition(RecordPositionModel recordPositionModel)
        {
           
            var vehicleUser = await this._vehicleUserService.GetVehicleUserInformationByUsername(recordPositionModel.UserName);
            if (vehicleUser == null)
                return false;

            List<VehicleLocation> vehicleLocationList = vehicleUser.vehicleInfo.locations;

            VehicleLocation vehicleLocation = new VehicleLocation();
            vehicleLocation.Id = Guid.NewGuid().ToString();
            vehicleLocation.latitude = recordPositionModel.Latitude;
            vehicleLocation.longitude = recordPositionModel.Longitude;
            vehicleLocation.timestamp = recordPositionModel.Timestamp;
            vehicleLocationList.Add(vehicleLocation);

            vehicleUser.vehicleInfo.locations = vehicleLocationList;

            await this._vehicleLocationRepository.UpdateAsync(vehicleUser);
            return true;
        }
    }
}
