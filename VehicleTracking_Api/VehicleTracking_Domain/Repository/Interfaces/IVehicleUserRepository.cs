using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Repository.Interfaces
{
  
    public interface IVehicleUserRepository : IDataRepository<VehicleUser>
    {
        
    }
}
