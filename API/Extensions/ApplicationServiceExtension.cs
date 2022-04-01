using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;
public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config) 
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        services.AddDbContext<DataContext>(options => {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}