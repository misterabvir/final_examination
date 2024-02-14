using Messenger.Shared.Security.Base;
using Messenger.Shared.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

namespace Messenger.Shared;

public static class DependencyInjection
{
    /// <summary>
    /// Adds token manager services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve configuration settings.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSharedDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add authentication services
        services.AddAuth(configuration);

        // Add Swagger documentation services
        services.AddSwaggerService();

        // Add token service as a singleton
        services.AddSingleton<ITokenService, TokenService>();

        return services;
    }

    /// <summary>
    /// Adds authentication services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the authentication services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> from which to retrieve JwtConfiguration.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    private static IServiceCollection AddAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Retrieves JwtConfiguration from the specified IConfiguration
        var jwt = configuration.GetSection("JwtConfiguration").Get<JwtConfiguration>()
            ?? throw new Exception("Jwt configuration not found");

        // Registers the JwtConfiguration as a singleton service
        services.AddSingleton(provider => jwt);

        // Adds authorization services
        services.AddAuthorization();

        // Adds JWT authentication services
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,
                    IssuerSigningKey = jwt.PublicKey
                };
            });

        return services;
    }

    /// <summary>
    /// Adds Swagger service to the specified collection.
    /// </summary>
    /// <param name="services">The service collection to which Swagger service will be added.</param>
    /// <returns>The modified service collection with added Swagger service.</returns>
    private static IServiceCollection AddSwaggerService(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "Jwt Token",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                },
                new List<string>()
            }
            });
        });

        return services;
    }
}
