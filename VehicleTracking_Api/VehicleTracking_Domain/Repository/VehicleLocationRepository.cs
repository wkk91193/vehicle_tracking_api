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
    public class VehicleLocationRepository : CosmosDbDataRepository<VehicleUserEntity>, IVehicleLocationRepository
    {
        public VehicleLocationRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                CosmosClient client) : base(cosmosDbConfiguration, client)
        {
        }

        public override string ContainerName => _cosmosDbConfiguration.ContainerName;

        public async Task<VehicleInformationEntity> GetLatestLocationOfVehicle(string vehicleReg)
        {
            Container container = GetContainer();
            var entities = new List<VehicleInformationEntity>();
            QueryDefinition queryDefinition = new QueryDefinition("select c.vehicleInfo from c WHERE c.vehicleInfo.vehicleReg=@vehicleReg")
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
            VehicleInformationEntity vehicleInformation = entities.FirstOrDefault();
            List<VehicleLocationEntity> locationList = entities.FirstOrDefault().LocationList;
            locationList.Sort((x, y) => DateTime.Compare(y.Timestamp, x.Timestamp));
            locationList.RemoveRange(1, locationList.Count - 1); 
            vehicleInformation.LocationList = locationList;
            return vehicleInformation;
           
        }
    }
}
