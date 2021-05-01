using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Domain.Entities;

namespace VehicleTracking_Domain.Repository.Interfaces
{
    public interface IVehicleLocationRepository:IDataRepository<VehicleUserEntity>
    {
        public Task<VehicleInformationEntity> GetLatestLocationOfVehicle(string vehicleReg);
    }
}
