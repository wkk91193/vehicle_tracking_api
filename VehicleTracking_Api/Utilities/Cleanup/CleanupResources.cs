using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTracking_Data.Identity;

namespace VehicleTracking_Api.Utilities.Cleanup
{
    public  class CleanupResources
    {   
        public static async Task RemoveUserFromDBAsync(ApplicationUser applicationUser, UserManager<ApplicationUser> userManager)
        {
            var appUser = await userManager.FindByNameAsync(applicationUser.UserName);
            if (appUser != null)
            {
                await userManager.DeleteAsync(appUser);
            }
        }
    }
}
