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

    public class VehicleUserRepository : CosmosDbDataRepository<VehicleUserEntity>, IVehicleUserRepository
    {
        public VehicleUserRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                 CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.ContainerName;

        public async Task<bool> CheckVehicleAlreadyRegistered(string vehicleReg)
        {
            Container container = GetContainer();
            var entities = new List<VehicleInformationEntity>();
            QueryDefinition queryDefinition = new QueryDefinition("select * from c WHERE c.vehicleInfo.vehicleReg=@vehicleReg")
                .WithParameter("@vehicleReg", vehicleReg);
        
            using (FeedIterator<VehicleInformationEntity> queryResultSetIterator = container.GetItemQueryIterator<VehicleInformationEntity>(queryDefinition))
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<VehicleInformationEntity> response = await queryResultSetIterator.ReadNextAsync();
                    foreach (var entity in response)
                    {
                        entities.Add(entity);
                    }
                }
            }
            return entities.Count > 0 ? true : false;

        }

        public async Task<VehicleUserEntity> GetVehicleUserInformationByUsername(string userName)
        {

            Container container = GetContainer();
            var entities = new List<VehicleUserEntity>();
            QueryDefinition queryDefinition = new QueryDefinition("select * from c where c.email= @username")
                .WithParameter("@username", userName);
          
            using (FeedIterator<VehicleUserEntity> queryResultSetIterator = container.GetItemQueryIterator<VehicleUserEntity>(queryDefinition))
            {
                while (queryResultSetIterator.HasMoreResults)
                {
                    FeedResponse<VehicleUserEntity> response = await queryResultSetIterator.ReadNextAsync();
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
