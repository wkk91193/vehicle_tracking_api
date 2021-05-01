using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services.Interfaces
{
    public interface IVehicleLocationService
    {
        public Task<bool> RecordPosition(RecordPositionModel recordPositionModel);

        public Task<VehicleInformationModel> GetLatestLocationOfVehicle(string vehicleReg);

        public Task<VehicleInformationModel> GetLocationForVehicleForGivenTime(string vehicleReg,string lowerTimeBound, string upperTimeBound);

    }
}
