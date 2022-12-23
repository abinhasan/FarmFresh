using Microsoft.AspNetCore.Identity;
using SuperMarket.Domain.Entities.Entities;
using SuperMarket.Domain.Enums;

namespace SuperMarket.Infrastructure.Data.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Super User
            var defaultUser = new ApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "Abin",
                LastName = "Hasan",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true,
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.SuperAdmin.ToString());
                }

            }
        }
    }
}
