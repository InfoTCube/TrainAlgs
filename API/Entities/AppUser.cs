using Microsoft.AspNetCore.Identity;

namespace API.Entities;
public class AppUser : IdentityUser<int>
{
    public ICollection<AppUserRole>? UserRoles { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public string? Country { get; set; }
    public string? Description { get; set; }
    public ICollection<AlgTask>? Tasks { get; set; }
}