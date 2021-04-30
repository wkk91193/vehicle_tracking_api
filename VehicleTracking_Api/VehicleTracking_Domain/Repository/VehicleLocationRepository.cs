using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Domain.Configuration.Interfaces;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Domain.Repository.Interfaces;

namespace VehicleTracking_Domain.Repository
{
    public class VehicleLocationRepository : CosmosDbDataRepository<VehicleUser>, IVehicleLocationRepository
    {
        public VehicleLocationRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.ContainerName;
    }
}
