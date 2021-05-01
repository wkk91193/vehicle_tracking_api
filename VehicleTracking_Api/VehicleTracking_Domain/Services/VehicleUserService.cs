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

        public async Task<VehicleUserEntity> GetVehicleUserInformationByUsername(string userName)
        {
            var result = await this._vehicleUserRepository.GetVehicleUserInformationByUsername(userName);
            return result;

        }

        public async Task SaveAdminToCosmo(RegisterAdminUserModel userModel)
        {
            VehicleUserEntity vehicleUserEntity = new VehicleUserEntity();
            vehicleUserEntity.Id = Guid.NewGuid().ToString();
            vehicleUserEntity.FirstName = userModel.FirstName;
            vehicleUserEntity.LastName = userModel.LastName;
            vehicleUserEntity.Email = userModel.UserName;
            vehicleUserEntity.RoleType = ApplicationUserRoles.Admin;

            VehicleInformationEntity vehicleInfoEntity = new VehicleInformationEntity();
            vehicleUserEntity.VehicleInfo = vehicleInfoEntity;

            await this._vehicleUserRepository.AddAsync(vehicleUserEntity);

        }

        public async Task<bool> SaveUserToCosmo(RegisterUserModel userModel)
        {
            if (await this.CheckVehicleAlreadyRegistered(userModel.VehicleInfo.VehicleReg))
            {
                return false;
            }
            VehicleUserEntity vehicleUserEntity = new VehicleUserEntity();
            vehicleUserEntity.Id= Guid.NewGuid().ToString();
            vehicleUserEntity.FirstName = userModel.FirstName;
            vehicleUserEntity.LastName = userModel.LastName;
            vehicleUserEntity.Email = userModel.UserName;
            vehicleUserEntity.RoleType = ApplicationUserRoles.User;

            VehicleInformationEntity vehicleInfoEntity = new VehicleInformationEntity();
            vehicleInfoEntity.Id = Guid.NewGuid().ToString();
            vehicleInfoEntity.VehicleReg = userModel.VehicleInfo.VehicleReg;

            List<VehicleLocationEntity> locationList = new List<VehicleLocationEntity>();
            vehicleInfoEntity.LocationList = locationList;
            vehicleUserEntity.VehicleInfo = vehicleInfoEntity;
           
            await this._vehicleUserRepository.AddAsync(vehicleUserEntity);
            return true;
                            
        }
    }
}
