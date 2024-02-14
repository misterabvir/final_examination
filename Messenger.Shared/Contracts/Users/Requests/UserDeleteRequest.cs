namespace Messenger.Shared.Contracts.Users.Requests;

/// <summary>
/// Represents the request model for remove user.
/// </summary>
public class UserDeleteRequest
{
    /// <summary>
    /// Unique identifier of the user. 
    /// </summary>
    public required Guid Id { get; init; }

}
