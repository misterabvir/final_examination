using Messenger.Shared.Abstractions.Repositories;
using Messenger.UserManager.Domain;

namespace Messenger.UserManager.DataAccessLayer.Repositories.Base;

/// <summary>
/// Interface for interacting with user entities in the repository.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>
    /// Retrieves all users asynchronously from the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>An enumerable collection of users.</returns>
    Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a user entity from the repository based on its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the user to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, returning a user entity if found. Returns null if no user is found.</returns>
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves a user entity from the repository based on its email asynchronously.
    /// </summary>
    /// <param name="email">The email of the user to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    /// <returns>A task representing the asynchronous operation, returning a user entity if found. Returns null if no user matches the specified email.</returns>
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new user entity in the repository asynchronously.
    /// </summary>
    /// <param name="user">The user entity to create.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    Task CreateAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Updates an existing user entity in the repository asynchronously.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    Task UpdateAsync(User user, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a user entity from the repository based on its unique identifier asynchronously.
    /// </summary>
    /// <param name="user">The user entity to delete.</param>">
    /// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
    Task DeleteAsync(User user, CancellationToken cancellationToken);
}
