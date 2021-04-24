using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VehicleTracking_Domain.Configuration.Interfaces;
using VehicleTracking_Domain.Repository.Interfaces;
using VehicleTracking_Domain.Entities;
using VehicleTracking_Domain.Repository;
using VehicleTracking_Domain.Services.Interfaces;
using VehicleTracking_Domain.Services;

namespace VehicleTracking_Api.Controller.DependencyInjection
{
    public static class CosmosDataCollectionExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {

          services.TryAddSingleton(implementationFactory =>
            {
                var cosmoDbConfiguration = implementationFactory.GetRequiredService<ICosmosDbConfiguration>();
                CosmosClient cosmosClient = new CosmosClient(cosmoDbConfiguration.ConnectionString, new CosmosClientOptions()
                {
                    SerializerOptions = new CosmosSerializationOptions()
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }

                });
                Database database = cosmosClient.CreateDatabaseIfNotExistsAsync(cosmoDbConfiguration.DatabaseName)
                                                       .GetAwaiter()
                                                       .GetResult();
                database.CreateContainerIfNotExistsAsync(
                    cosmoDbConfiguration.ContainerName,
                    cosmoDbConfiguration.PartitionKeyPath,
                    400)
                    .GetAwaiter()
                    .GetResult();
            
                return cosmosClient;
            });

            services.AddSingleton<IVehicleUserRepository, VehicleUserRepository>();
            //services.AddSingleton<IDataRepository<VehiclePosition>, VehiclePositionRepository>();
            services.AddSingleton<IVehicleUserService, VehicleUserService>();

            return services;
        }
    }
}
