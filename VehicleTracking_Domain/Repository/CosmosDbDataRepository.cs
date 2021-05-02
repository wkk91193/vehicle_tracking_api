using Azure;
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
    public abstract class CosmosDbDataRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected readonly ICosmosDbConfiguration _cosmosDbConfiguration;
        protected readonly CosmosClient _client;

        public abstract string ContainerName { get; }

        public CosmosDbDataRepository(ICosmosDbConfiguration cosmosDbConfiguration,
                           CosmosClient client)
        {
            _cosmosDbConfiguration = cosmosDbConfiguration
                    ?? throw new ArgumentNullException(nameof(cosmosDbConfiguration));

            _client = client
                    ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<T> AddAsync(T newEntity)
        {

            Container container = GetContainer();
            ItemResponse<T> createResponse = await container.CreateItemAsync(newEntity);
            return createResponse.Resource;

        }

        public async Task DeleteAsync(string entityId)
        {

            Container container = GetContainer();
            await container.DeleteItemAsync<T>(entityId, new PartitionKey(entityId));

        }

        public async Task<T> GetAsync(string entityId)
        {
            Container container = GetContainer();

            ItemResponse<T> entityResult = await container.ReadItemAsync<T>(entityId, new PartitionKey(entityId));
            return entityResult.Resource;

        }

        public async Task<T> UpdateAsync(T entity)
        {

            Container container = GetContainer();
            await container.ReplaceItemAsync(entity, entity.Id.ToString(), new PartitionKey(entity.Id.ToString()));          
            return entity;

        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {

            Container container = GetContainer();
            FeedIterator<T> queryResultSetIterator = container.GetItemQueryIterator<T>();
            List<T> entities = new List<T>();
            using (FeedIterator<T> feedIterator = container.GetItemQueryIterator<T>())
            {
                while (feedIterator.HasMoreResults)
                {
                    FeedResponse<T> response = await feedIterator.ReadNextAsync();
                    foreach (var entity in response)
                    {
                        entities.Add(entity);
                    }
                }
            }

            return entities;

           
        }


        protected Container GetContainer()
        {
            var database = _client.GetDatabase(_cosmosDbConfiguration.DatabaseName);
            var container = database.GetContainer(ContainerName);
            return container;
        }
    }
}
