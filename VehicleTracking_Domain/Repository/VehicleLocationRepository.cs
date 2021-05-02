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
            var entities = new List<VehicleUserEntity>();
            QueryDefinition queryDefinition = new QueryDefinition("select c.vehicleInfo from c WHERE c.vehicleInfo.vehicleReg=@vehicleReg")
                .WithParameter("@vehicleReg", vehicleReg);

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
            if (entities.Count == 0 || entities.FirstOrDefault().VehicleInfo.Locations.Count==0)
            {
                return null;
            }

            VehicleInformationEntity vehicleInformation = entities.FirstOrDefault().VehicleInfo;
            List<VehicleLocationEntity> locationList = vehicleInformation.Locations;
            locationList.Sort((x, y) => DateTime.Compare(y.Timestamp, x.Timestamp));
            locationList.RemoveRange(1, locationList.Count - 1); 
            vehicleInformation.Locations = locationList;
            return vehicleInformation;
           
        }

        public async Task<VehicleInformationEntity> GetLocationForVehicleForGivenTime(string vehicleReg, string lowerTimeBound, string upperTimeBound)
        {
            Container container = GetContainer();
            var entities = new List<VehicleUserEntity>();
            QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c " +
                                                                  "WHERE EXISTS (SELECT VALUE s FROM s " +
                                                                  "IN c.vehicleInfo.locations WHERE s.timestamp >= @lowerTimeBound " +
                                                                  "AND s.timestamp <= @upperTimeBound AND c.vehicleInfo.vehicleReg=@vehicleReg)")
                .WithParameter("@vehicleReg", vehicleReg)
                .WithParameter("@lowerTimeBound", lowerTimeBound)
                .WithParameter("@upperTimeBound", upperTimeBound);

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
            if (entities.Count == 0)
            {
                return null;
            }
             return entities.FirstOrDefault().VehicleInfo;
            
        }
    }
}
