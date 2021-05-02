using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking_Domain.Configuration;
using VehicleTracking_Domain.Configuration.Interfaces;
using VehicleTracking_Domain.Services;
using VehicleTracking_Domain.Services.Interfaces;

namespace VehicleTracking_Api.DependancyInjection
{
   
    public static class ConfigurationServiceCollectionExtensions
    {
        public static IServiceCollection AddAppConfiguration(this IServiceCollection services, IConfiguration config)
        {
           
            services.Configure<CosmosDbConfiguration>(config.GetSection("CosmosDbSettings"));
            services.AddSingleton<IValidateOptions<CosmosDbConfiguration>, CosmosDbConfigurationValidation>();
            var cosmosDbConfiguration = services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbConfiguration>>().Value;
            services.AddSingleton<ICosmosDbConfiguration>(cosmosDbConfiguration);
            services.AddSingleton<IExternalService, ExternalService>();
            return services;
        }
    }
}
