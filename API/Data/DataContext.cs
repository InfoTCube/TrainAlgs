using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
    AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext()
    {
    }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AlgTask> Tasks { get; set; }
    public DbSet<Solution> Solutions { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Competition> Competitions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        builder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        builder.Entity<AppUserCompetition>()
            .HasKey(uc => new { uc.AppUserId, uc.CompetitionId });  
        builder.Entity<AppUserCompetition>()
            .HasOne(uc => uc.User)
            .WithMany(au => au.CompetitionsParticipated)
            .HasForeignKey(uc => uc.AppUserId);  
        builder.Entity<AppUserCompetition>()
            .HasOne(uc => uc.Competition)
            .WithMany(c => c.Participants)
            .HasForeignKey(uc => uc.CompetitionId);
    }
}