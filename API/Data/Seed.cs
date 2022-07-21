using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class Seed
{
    public static async Task SeedRoles(RoleManager<AppRole> roleManager) 
    {
        if(roleManager.Roles.Any()) return;

        var roles = new List<AppRole> 
        {
            new AppRole{Name = "Member"},
            new AppRole{Name = "Moderator"},
            new AppRole{Name = "Admin"}
        };
            
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }
}