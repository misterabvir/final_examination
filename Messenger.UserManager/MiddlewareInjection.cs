namespace Messenger.UserManager;

public static class MiddlewareInjection
{
    /// <summary>
    /// Configures the application to use the user manager.
    /// </summary>
    /// <param name="app">The web application builder.</param>
    /// <returns>The configured web application builder.</returns>
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
