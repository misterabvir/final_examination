namespace Messenger.Shared.Contracts.Users.Responses;

/// <summary>
/// Represents the response model for user.
/// </summary>
public class UserResponse
{
    /// <summary>
    /// Unique identifier of the user.
    /// </summary>
    public required Guid Id { get; init; }
    /// <summary>
    /// Email address of the user.
    /// </summary>
    public required string Email { get; init; }

    /// <summary>
    /// Role of the user.
    /// </summary>
    public required string Role { get; init; }
}

