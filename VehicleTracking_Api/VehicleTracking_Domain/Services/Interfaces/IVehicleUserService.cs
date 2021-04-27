using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VehicleTracking_Data.Identity;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services.Interfaces
{
    public interface IVehicleUserService
    {
        public Task<bool> SaveUserToCosmo(RegisterUserModel userModel, string roleType);
        public Task<VehicleUser> GetVehicleUserInformationByUsername(string userName);
        public Task<bool> CheckVehicleAlreadyRegistered(string vehicleReg);


    }
}
