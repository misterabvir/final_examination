using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Users.Requests;
using Messenger.Shared.Contracts.Users.Responses;

namespace Messenger.UserManager.BusinessLogicalLayer.Services.Base;

/// <summary>
/// Interface for user service operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Logs in a user.
    /// </summary>
    /// <param name="request">The user authentication request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>A result containing either the authentication token on success or an error on failure.</returns>
    Task<Result<UserTokenResponse, Error>> Login(UserAuthRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Registers a new user.
    /// </summary>
    /// <param name="request">The user authentication request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>A result containing either the authentication token on success or an error on failure.</returns>
    Task<Result<UserTokenResponse, Error>> Register(UserAuthRequest request, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>A result containing either a collection of user responses on success or an error on failure.</returns>
    Task<Result<IEnumerable<UserResponse>, Error>> GetAll(CancellationToken cancellationToken);

    /// <summary>
    /// Deletes a user.
    /// </summary>
    /// <param name="request">The user deletion request.</param>
    /// <param name="cancellationToken">A token to cancel the asynchronous operation if needed.</param>
    /// <returns>A result containing either a success message or an error on failure.</returns>
    Task<Result<UserDeleteResponse, Error>> Delete(UserDeleteRequest request, CancellationToken cancellationToken);
}

