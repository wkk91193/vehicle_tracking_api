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

        public async Task<VehicleInformationModel> GetLatestLocationOfVehicle(string vehicleReg)
        {
            VehicleInformationEntity vehicleInformation = await _vehicleLocationRepository.GetLatestLocationOfVehicle(vehicleReg);

            VehicleInformationModel vehicleInformationModel = new VehicleInformationModel();
            vehicleInformationModel.VehicleReg = vehicleInformation.VehicleReg;

            List<VehicleLocationModel> vehicleLocationModelList = new List<VehicleLocationModel>();
            VehicleLocationModel vehicleLocationModel = new VehicleLocationModel();
            vehicleLocationModel.Latitude = vehicleInformation.LocationList[0].Latitude;
            vehicleLocationModel.Longitude = vehicleInformation.LocationList[0].Longitude; 
            vehicleLocationModel.Timestamp = vehicleInformation.LocationList[0].Timestamp.ToString();
            vehicleLocationModelList.Add(vehicleLocationModel);

            vehicleInformationModel.VehicleLocations = vehicleLocationModelList;
            return vehicleInformationModel;
        }

        public async Task<bool> RecordPosition(RecordPositionModel recordPositionModel)
        {
           
            var vehicleUser = await this._vehicleUserService.GetVehicleUserInformationByUsername(recordPositionModel.UserName);
            if (vehicleUser == null)
                return false;

            List<VehicleLocationEntity> vehicleLocationList = vehicleUser.VehicleInfo.LocationList;

            VehicleLocationEntity vehicleLocation = new VehicleLocationEntity();
            vehicleLocation.Id = Guid.NewGuid().ToString();
            vehicleLocation.Latitude = recordPositionModel.Latitude;
            vehicleLocation.Longitude = recordPositionModel.Longitude;
            vehicleLocation.Timestamp = DateTime.Parse(recordPositionModel.Timestamp);
            vehicleLocationList.Add(vehicleLocation);

            vehicleUser.VehicleInfo.LocationList = vehicleLocationList;

            await this._vehicleLocationRepository.UpdateAsync(vehicleUser);
            return true;
        }
    }
}
