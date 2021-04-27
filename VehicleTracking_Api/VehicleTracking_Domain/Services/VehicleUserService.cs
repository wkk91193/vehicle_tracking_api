using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleTracking_Data.Identity;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Domain.Repository.Interfaces;
using VehicleTracking_Domain.Services.Interfaces;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services
{
    public class VehicleUserService : IVehicleUserService {
        private readonly IVehicleUserRepository _vehicleUserRepository;

        public VehicleUserService(IVehicleUserRepository vehicleUserRepository)
        {
            this._vehicleUserRepository = vehicleUserRepository
                                      ?? throw new ArgumentNullException(nameof(vehicleUserRepository));
          
        }

        public async Task<bool> CheckVehicleAlreadyRegistered(string vehicleReg)
        {
            var result = await this._vehicleUserRepository.CheckVehicleAlreadyRegistered(vehicleReg);
            return result;

        }

        public async Task<VehicleUser> GetVehicleUserInformationByUsername(string userName)
        {
            var result = await this._vehicleUserRepository.GetVehicleUserInformationByUsername(userName);
            return result;

        }

        public async Task<bool> SaveUserToCosmo(RegisterUserModel userModel, string roleType)
        {
            if (await this.CheckVehicleAlreadyRegistered(userModel.VehicleInfo.VehicleReg))
                return false;

            VehicleUser vehicleUser = new VehicleUser();
            vehicleUser.Id= Guid.NewGuid().ToString();
            vehicleUser.firstName = userModel.FirstName;
            vehicleUser.lastName = userModel.LastName;
            vehicleUser.email = userModel.UserName;
            vehicleUser.roleType = roleType;

            Entities.VehicleInformation vehicleInfo = new Entities.VehicleInformation();
            vehicleInfo.Id = Guid.NewGuid().ToString();
            vehicleInfo.vehicleReg = userModel.VehicleInfo.VehicleReg;

            List<VehicleLocation> locationList = new List<VehicleLocation>();
            vehicleInfo.locations = locationList;
 
         
            vehicleUser.vehicleInfo = vehicleInfo;
           
            var result = await this._vehicleUserRepository.AddAsync(vehicleUser);
            if (result == null)
                return false;

            return true;
               
              
        }
    }
}
