using Messenger.UserManager.DataAccessLayer.Contexts;
using Messenger.UserManager.DataAccessLayer.Repositories.Base;
using Messenger.UserManager.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.UserManager.DataAccessLayer.Repositories;

/// <summary>
/// Internal implementation of the IUserRepository interface.
/// </summary>
internal sealed class UserRepository : IUserRepository
{
    private readonly UserManagerContext _context;

    public UserRepository(UserManagerContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all users asynchronously from the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An enumerable collection of users.</returns>
    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Users
            .Include(u => u.Role)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a new user asynchronously.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a user asynchronously based on its unique identifier.
    /// </summary>
    /// <param name="user">The user entity to delete.</param>">
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    public async Task DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a user by email asynchronously.
    /// </summary>
    /// <param name="email">The email of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await _context.Users.Include(u => u.Role).SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    /// <summary>
    /// Retrieves a user by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>The user entity if found; otherwise, null.</returns>
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Users.Include(u => u.Role).AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    /// <summary>
    /// Updates an existing user asynchronously.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    public async Task UpdateAsync(User user, CancellationToken cancellationToken)
    {
        var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id, cancellationToken);
        if (userToUpdate is not null)
        {
            userToUpdate.Email = user.Email;
            userToUpdate.Password = user.Password;
            userToUpdate.Salt = user.Salt;
            userToUpdate.Role = user.Role;
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
