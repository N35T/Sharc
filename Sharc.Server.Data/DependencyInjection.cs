using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharc.Server.Data.Persistence;

namespace Sharc.Server.Data; 

public static class DependencyInjection {

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config) {

        services.AddDbContext<ApplicationDbContext>(opt => {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        
        return services;
    }
}