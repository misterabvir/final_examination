using Messenger.Shared;
using Messenger.MessageManager.BusinessLogicalLayer;
using Messenger.MessageManager.DataAccessLayer;
using MassTransit;


namespace Messenger.MessageManager;

public static class DependencyInjection
{
    /// <summary>
    /// Adds message manager services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to configure the services.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddMessageManager(this IServiceCollection services, IConfiguration configuration)
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

        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();
            busConfigurator.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host => {
                    host.Username(configuration["MessageBroker:Username"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                configurator.ConfigureEndpoints(context);
            });
            busConfigurator.AddConsumers(typeof(DependencyInjection).Assembly);
        });

        return services;
    }
}
