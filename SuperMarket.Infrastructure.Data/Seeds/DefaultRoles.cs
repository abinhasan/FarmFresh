using Microsoft.AspNetCore.Identity;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Domain.Enums;

namespace SuperMarket.Infrastructure.Data.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Roles.Basic.ToString()));
        }
    }
}
