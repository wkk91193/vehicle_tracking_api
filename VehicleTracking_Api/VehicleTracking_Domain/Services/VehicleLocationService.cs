using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private readonly IExternalService _externalService;

        public VehicleLocationService(IVehicleLocationRepository vehicleLocationRepository,
                                      IVehicleUserService vehicleUserService,
                                      IExternalService externalService)

        {
            this._vehicleLocationRepository = vehicleLocationRepository
                                      ?? throw new ArgumentNullException(nameof(vehicleLocationRepository));

            this._vehicleUserService = vehicleUserService
                                     ?? throw new ArgumentNullException(nameof(vehicleUserService));
            this._externalService = externalService
                                     ?? throw new ArgumentNullException(nameof(externalService));

        }

        public async Task<VehicleInformationModel> GetLatestLocationOfVehicle(string vehicleReg)
        {
            VehicleInformationEntity vehicleInformation = await _vehicleLocationRepository.GetLatestLocationOfVehicle(vehicleReg);
            if (vehicleInformation == null)
                return null;

            VehicleInformationModel vehicleInformationModel = new VehicleInformationModel();
            vehicleInformationModel.VehicleReg = vehicleInformation.VehicleReg;

            List<VehicleLocationModel> vehicleLocationModelList = new List<VehicleLocationModel>();
            VehicleLocationModel vehicleLocationModel = new VehicleLocationModel();
            vehicleLocationModel.Latitude = vehicleInformation.Locations[0].Latitude;
            vehicleLocationModel.Longitude = vehicleInformation.Locations[0].Longitude;
            vehicleLocationModel.Timestamp = vehicleInformation.Locations[0].Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
            vehicleLocationModel.AddressOfLocation = await this._externalService.GetAddressFromCoordinates(vehicleLocationModel.Latitude, vehicleLocationModel.Longitude); 
            vehicleLocationModelList.Add(vehicleLocationModel);

            vehicleInformationModel.VehicleLocations = vehicleLocationModelList;
            return vehicleInformationModel;
        }

        public async Task<VehicleInformationModel> GetLocationForVehicleForGivenTime(string vehicleReg, string lowerTimeBound, string upperTimeBound)
        {
            VehicleInformationEntity vehicleInformation = await _vehicleLocationRepository.GetLocationForVehicleForGivenTime(vehicleReg,lowerTimeBound,upperTimeBound);
            if (vehicleInformation == null)
                return null;

            VehicleInformationModel vehicleInformationModel = new VehicleInformationModel();
            vehicleInformationModel.VehicleReg = vehicleInformation.VehicleReg;

            List<VehicleLocationModel> vehicleLocationModelList = new List<VehicleLocationModel>();
            foreach (VehicleLocationEntity vehicleLocationEntity in vehicleInformation.Locations)
            {
                VehicleLocationModel vehicleLocationModel = new VehicleLocationModel();
                vehicleLocationModel.Latitude = vehicleLocationEntity.Latitude;
                vehicleLocationModel.Longitude = vehicleLocationEntity.Longitude;
                vehicleLocationModel.Timestamp = vehicleLocationEntity.Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
                vehicleLocationModel.AddressOfLocation = await this._externalService.GetAddressFromCoordinates(vehicleLocationModel.Latitude, vehicleLocationModel.Longitude);
                vehicleLocationModelList.Add(vehicleLocationModel);

            }
            vehicleInformationModel.VehicleLocations = vehicleLocationModelList;
            return vehicleInformationModel;
            
        }

        public async Task<bool> RecordPosition(RecordPositionModel recordPositionModel)
        {
           
            var vehicleUser = await this._vehicleUserService.GetVehicleUserInformationByUsername(recordPositionModel.UserName);
            if (vehicleUser == null)
                return false;

            List<VehicleLocationEntity> vehicleLocationList = vehicleUser.VehicleInfo.Locations;

            VehicleLocationEntity vehicleLocation = new VehicleLocationEntity();
            vehicleLocation.Id = Guid.NewGuid().ToString();
            vehicleLocation.Latitude = recordPositionModel.Latitude;
            vehicleLocation.Longitude = recordPositionModel.Longitude;
            vehicleLocation.Timestamp = DateTime.Parse(recordPositionModel.Timestamp);
            vehicleLocationList.Add(vehicleLocation);

            vehicleUser.VehicleInfo.Locations = vehicleLocationList;

            await this._vehicleLocationRepository.UpdateAsync(vehicleUser);
            return true;
        }
    }
}
