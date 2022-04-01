using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data;
public class Seed
{
    public static async Task SeedRoles(UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager) 
    {
        if(roleManager.Roles.Any()) return;

        var roles = new List<AppRole> 
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Admin"}
        };
            
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }
}