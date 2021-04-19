using System;
using System.Threading.Tasks;
using VehicleTracking_Domain.Services.Interfaces;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services
{
    public class UserService : IUserService
    {
        public Task<UserModel> RegisterUser(UserModel userModel)
        {
            throw new NotImplementedException();
        }
    }
}
