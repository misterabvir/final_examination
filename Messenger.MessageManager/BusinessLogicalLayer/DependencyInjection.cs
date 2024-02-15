using Messenger.MessageManager.BusinessLogicalLayer.Services;
using Messenger.MessageManager.BusinessLogicalLayer.Services.Base;

namespace Messenger.MessageManager.BusinessLogicalLayer;

public static class DependencyInjection
{
    /// <summary>
    /// Adds the business logical layer services to the specified collection of services.
    /// </summary>
    /// <param name="services">The collection of services to add the business logical layer services to.</param>
    /// <returns>The same collection of services so that multiple calls can be chained.</returns>
    public static IServiceCollection AddBusinessLogicalLayer(this IServiceCollection services)
    {
        // Add AutoMapper with the assemblies from DependencyInjection
        services.AddAutoMapper(config => config.AddMaps(typeof(DependencyInjection).Assembly));

        // Add HttpClient as a singleton
        services.AddSingleton<HttpClient>();

        // Add IMessageService as a transient service
        services.AddTransient<IMessageService, MessageService>();

        return services;
    }
}
