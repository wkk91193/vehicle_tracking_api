using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking_Data.DbContext;
using VehicleTracking_Data.Identity;

namespace VehicleTracking_Api.SeedData
{
    public static class FirstRunSeedData
    {
        public static async Task CreateFirstAdminUser(IServiceProvider serviceProvider, IConfiguration config)
        {

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
           var userExists = await userManager.FindByNameAsync(config["FirstAdminUserData:Username"]);
            if (userExists==null)
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
            }
        }
    }
}
