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
        services.AddAutoMapper(config => config.AddMaps(typeof(DependencyInjection).Assembly));

        services.AddSingleton<HttpClient>();

        services.AddTransient<IHttpClientService, HttpClientService>();
        services.AddTransient<IMapperService, MapperService>();
        services.AddTransient<IMessageService, MessageService>();

        return services;
    }
}
