using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using VehicleTracking_Api.SeedData;

namespace VehicleTracking_Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await SeedDatabase(host);
            host.Run();

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders();
                // We have to be precise on the logging levels
                logging.AddConsole();
                logging.AddDebug();
                logging.AddAzureWebAppDiagnostics();
            })
            .ConfigureServices(services =>
            {
                services.Configure<AzureFileLoggerOptions>(options =>
                {
                    options.FileName = "my-azure-diagnostics-";
                    options.FileSizeLimit = 50 * 1024;
                    options.RetainedFileCountLimit = 5;
                });
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                    webBuilder.UseStartup<Startup>();
            });

        private static async Task SeedDatabase(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {

                    var config = services.GetRequiredService<IConfiguration>();
                    await FirstRunSeedData.CreateFirstAdminUser(scope.ServiceProvider, config);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError("An error occurred while seeding the database");
                }
            }
        }
    }
}
