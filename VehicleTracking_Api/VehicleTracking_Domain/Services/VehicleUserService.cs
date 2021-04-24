using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
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

        public async Task<RegisterUserModel> SaveUserToCosmo(RegisterUserModel userModel, string roleType)
        {
            //throw new NotImplementedException();
            VehicleUser vehicleUser = new VehicleUser();
            vehicleUser.firstName = userModel.FirstName;
            vehicleUser.lastName = userModel.LastName;
            vehicleUser.email = userModel.UserName;
            vehicleUser.roleType = roleType;
            vehicleUser.vehicleInfo = new List<VehiclePosition>();
            vehicleUser.Id = Guid.NewGuid().ToString();
            var result = await this._vehicleUserRepository.AddAsync(vehicleUser);
            if (result != null)
            {
                RegisterUserModel registerUserModel = new RegisterUserModel();

                registerUserModel.FirstName = result.firstName;
                registerUserModel.LastName = result.lastName;
                registerUserModel.UserName = result.email;
                return registerUserModel;
            }
            else
            {
                return null;
            }
        }
    }
}
