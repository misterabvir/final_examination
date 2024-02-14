using Messenger.UserManager.DataAccessLayer.Contexts;
using Messenger.UserManager.DataAccessLayer.Repositories;
using Messenger.UserManager.DataAccessLayer.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Messenger.UserManager.DataAccessLayer;

/// <summary>
/// Extension method for IServiceCollection to add dependencies related to the data access layer.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds dependencies related to the data access layer.
    /// </summary>
    /// <param name="services">The collection of services to add to.</param>
    /// <param name="configuration">The configuration to retrieve connection strings.</param>
    /// <returns>The modified service collection.</returns>
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure DbContext with connection string from configuration
        services.AddDbContext<UserManagerContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("UserManagerConnection")));

        // Register UserRepository and RoleRepository with transient lifetime
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();

        return services;
    }
}
