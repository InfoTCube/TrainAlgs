using Microsoft.AspNetCore.Identity;

namespace API.Entities;
public class AppUser : IdentityUser<int>
{
    public ICollection<AppUserRole>? UserRoles { get; set; }
}