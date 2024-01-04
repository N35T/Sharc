using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sharc.Core.Repositories;
using Sharc.Server.Data.Persistence;
using Sharc.Server.Data.Repositories;

namespace Sharc.Server.Data; 

public static class DependencyInjection {

    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration config) {

        services.AddDbContext<ApplicationDbContext>(opt => {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        services.AddTransient<IEventRepository, EventRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        
        return services;
    }
}