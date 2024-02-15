using Messenger.UserManager.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.UserManager.DataAccessLayer.Contexts;

/// <summary>
/// Defines a database context for the user manager.
/// </summary>
public sealed class UserManagerContext(DbContextOptions<UserManagerContext> options) : DbContext(options)
{

    /// <summary>
    /// Represents a collection of user entities in the database.
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// /// Represents a collection of role entities in the database.
    /// </summary>
    public DbSet<Role> Roles { get; set; } = null!;

    /// <summary>
    /// Overrides the method for model creation
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Applies all configurations from the assembly of the UserManagerContext class.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserManagerContext).Assembly);
    }
}
