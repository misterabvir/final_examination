namespace Messenger.MessageManager;

public static class MiddlewareInjection
{
    /// <summary>
    /// Configures the application to use the message manager.
    /// </summary>
    /// <param name="app">The web application instance.</param>
    /// <returns>The configured web application instance.</returns>
    public static WebApplication UseMessageManager(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        return app;
    }
}
