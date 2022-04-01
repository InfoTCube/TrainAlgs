using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions;
public static class IdentityServiceExtension
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<AppUser>(opt => 
        {
            opt.Password.RequireNonAlphanumeric = true;
        }).AddRoles<AppRole>()
            .AddRoleManager<RoleManager<AppRole>>()
            .AddSignInManager<SignInManager<AppUser>>()
            .AddRoleValidator<RoleValidator<AppRole>>()
            .AddEntityFrameworkStores<DataContext>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt => 
            {
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

        services.AddAuthorization(opt => 
        {
            opt.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
        });

        return services;
    }
}