using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Messenger.UserManager.DataAccessLayer.Contexts;

/// <summary>
/// Factory class for creating instances of the UserManagerContext during design time.
/// </summary>
public class UserManagerContextDesignTimeFactory : IDesignTimeDbContextFactory<UserManagerContext>
{
    /// <summary>
    /// Creates a new instance of the UserManagerContext for design-time tools.
    /// </summary>
    /// <param name="args">Arguments provided to the design-time factory. Not used in this implementation.</param>
    /// <returns>An instance of UserManagerContext configured for design-time use.</returns>
    public UserManagerContext CreateDbContext(string[] args)
    {
        // Create options for configuring the DbContext
        var optionsBuilder = new DbContextOptionsBuilder<UserManagerContext>();

        // Configure the DbContext to use PostgreSQL database
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=messenger_user_manager_db;Username=postgres;Password=password");

        // Create and return a new instance of UserManagerContext with the configured options
        return new UserManagerContext(optionsBuilder.Options);
    }
}
