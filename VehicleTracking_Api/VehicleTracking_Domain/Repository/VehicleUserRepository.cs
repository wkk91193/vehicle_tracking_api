using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleTracking_Domain.Configuration.Interfaces;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Domain.Repository.Interfaces;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Domain.Repository
{

    public class VehicleUserRepository : CosmosDbDataRepository<VehicleUser>, IVehicleUserRepository
    {
        public VehicleUserRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                 CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.ContainerName;

        public async Task<bool> CheckVehicleAlreadyRegistered(string vehicleReg)
        {
            Container container = GetContainer();
            var entities = new List<VehicleUser>();
            QueryDefinition queryDefinition = new QueryDefinition("select * from c WHERE c.vehicleInfo.vehicleReg= @vehicleReg")
                .WithParameter("@vehicleReg", vehicleReg);

            FeedIterator<VehicleUser> queryResultSetIterator = container.GetItemQueryIterator<VehicleUser>(queryDefinition);
            using (FeedIterator<VehicleUser> feedIterator = container.GetItemQueryIterator<VehicleUser>())
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<VehicleUser> response = await feedIterator.ReadNextAsync();
                    foreach (var entity in response)
                    {
                        entities.Add(entity);
                    }
                }
            }
            return entities.Count > 0 ? true : false;

        }

        async Task<VehicleUser> IVehicleUserRepository.GetVehicleUserInformationByUsername(string userName)
        {

            Container container = GetContainer();
            var entities = new List<VehicleUser>();
            QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.email= @username")
                .WithParameter("@username", userName);

            FeedIterator<VehicleUser> queryResultSetIterator = container.GetItemQueryIterator<VehicleUser>(queryDefinition);
            using (FeedIterator<VehicleUser> feedIterator = container.GetItemQueryIterator<VehicleUser>())
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<VehicleUser> response = await feedIterator.ReadNextAsync();
                    foreach (var entity in response)
                    {
                        entities.Add(entity);
                    }
                }
            }
            return entities.FirstOrDefault();

        }
    }
}
