using Messenger.Shared;
using Messenger.UserManager.BusinessLogicalLayer;
using Messenger.UserManager.DataAccessLayer;


namespace Messenger.UserManager;

public static class DependencyInjection
{
    public static IServiceCollection AddUserManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSharedDependencies(configuration);
        services.AddDataAccessLayer(configuration);
        services.AddBusinessLogicalLayer();

        return services;
    } 
}

public static class MiddlewareInjection
{
    public static WebApplication UseUserManager(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        return app;
    }
}
