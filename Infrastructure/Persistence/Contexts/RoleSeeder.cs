

using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence.Contexts;

public class RoleSeeder
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = new[] {"admin", "user"};
        foreach(var role in roles)
        {
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
