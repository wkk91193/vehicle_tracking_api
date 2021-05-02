using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using VehicleTracking_Data.Identity;
using VehicleTracking_Domain.Services.Interfaces;
using VehicleTracking_Models.Models;

namespace VehicleTracking_Api.SeedData
{
    public static class FirstRunSeedData
    {
        public static async Task CreateFirstAdminUser(IServiceProvider serviceProvider, IConfiguration config)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var usersService = serviceProvider.GetRequiredService<IVehicleUserService>();

            var userExists = await userManager.FindByNameAsync(config["FirstAdminUserData:Username"]);
            if (userExists == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = config["FirstAdminUserData:Username"],
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Email = config["FirstAdminUserData:Username"]
                };

                await userManager.CreateAsync(user, config["FirstAdminUserData:Password"]);

                if (!await roleManager.RoleExistsAsync(ApplicationUserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(ApplicationUserRoles.Admin));

                await userManager.AddToRoleAsync(user, ApplicationUserRoles.Admin);

                RegisterAdminUserModel registerAdminUserModel = new RegisterAdminUserModel();
                registerAdminUserModel.FirstName = config["FirstAdminUserData:FirstName"];
                registerAdminUserModel.LastName = config["FirstAdminUserData:LastName"];
                registerAdminUserModel.UserName = config["FirstAdminUserData:Username"];
                registerAdminUserModel.Password = config["FirstAdminUserData:Password"];
                await usersService.SaveAdminToCosmo(registerAdminUserModel);

            }
        }
    }
}
