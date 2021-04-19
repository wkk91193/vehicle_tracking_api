using System;
using System.Threading.Tasks;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserModel> RegisterUser(UserModel userModel);
    }
}
