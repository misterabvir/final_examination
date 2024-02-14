using Messenger.UserManager.BusinessLogicalLayer.Services;
using Messenger.UserManager.BusinessLogicalLayer.Services.Base;

namespace Messenger.UserManager.BusinessLogicalLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogicalLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddMaps(typeof(DependencyInjection).Assembly));
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IEncryptService, EncryptService>();

        return services;
    }
}
