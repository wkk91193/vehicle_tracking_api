using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VehicleTracking_Data.Identity;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services.Interfaces
{
    public interface IVehicleUserService
    {
        public Task<RegisterUserModel> SaveUserToCosmo(RegisterUserModel userModel, string roleType);


    }
}
