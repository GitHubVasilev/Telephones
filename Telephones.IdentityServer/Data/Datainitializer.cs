using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Telephones.IdentityServer.Entities;

namespace Telephones.IdentityServer.Data
{
    public static class DataInitializer
    {
        public static void Init(IServiceProvider scopeServiceProvider)
        {
            UserManager<ApplicationUser> userManager = scopeServiceProvider.GetService<UserManager<ApplicationUser>>();
            RoleManager<ApplicationRole> roleManager = scopeServiceProvider.GetService<RoleManager<ApplicationRole>>();

            if (userManager is null || userManager.FindByNameAsync("UserAdmin").Result is not null) return;

            ApplicationRole adminRole = new ApplicationRole 
            {
                Name = "Admin"
            };

            ApplicationRole userRole = new ApplicationRole
            {
                Name = "User"
            };

            IdentityResult resultRole1 = roleManager.CreateAsync(adminRole).Result;
            IdentityResult resultRole2 = roleManager.CreateAsync(userRole).Result;

            ApplicationUser userAdmin = new ApplicationUser
            {
                UserName = "UserAdmin",
            };
            ApplicationUser userUser = new ApplicationUser
            {
                UserName = "UserUser",
            };

            IdentityResult resultUser1 = userManager.CreateAsync(userAdmin, "123456").Result;
            if (resultUser1.Succeeded && resultRole1.Succeeded)
            {
                userManager.AddClaimAsync(userAdmin, new Claim(ClaimTypes.Role, "Admin")).GetAwaiter().GetResult();
            }

            IdentityResult resultUser2 = userManager.CreateAsync(userUser, "123456").Result;
            if (resultUser2.Succeeded && resultRole2.Succeeded)
            {
                userManager.AddClaimAsync(userUser, new Claim(ClaimTypes.Role, "User")).GetAwaiter().GetResult();
            }
        }
    }
}
