using Messenger.Shared;
using Messenger.UserManager.BusinessLogicalLayer;
using Messenger.UserManager.DataAccessLayer;


namespace Messenger.UserManager;

public static class DependencyInjection
{
/// <summary>
/// Adds user manager to the service collection
/// </summary>
/// <param name="services">The current service collection</param>
/// <param name="configuration">The configuration</param>
/// <returns>The updated service collection</returns>
public static IServiceCollection AddUserManager(this IServiceCollection services, IConfiguration configuration)
{
    // Add controllers
    services.AddControllers();

    // Add endpoint API explorer
    services.AddEndpointsApiExplorer();

    // Add shared dependencies
    services.AddSharedDependencies(configuration);

    // Add data access layer
    services.AddDataAccessLayer(configuration);

    // Add business logical layer
    services.AddBusinessLogicalLayer();

    return services;
}
}
