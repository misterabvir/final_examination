using System.ComponentModel.DataAnnotations;

namespace Messenger.Shared.Contracts.Users.Requests;

/// <summary>
/// Represents the request model for user authentication.
/// </summary>
public class UserAuthRequest
{
    /// <summary>
    /// Email address of the user.
    /// </summary>
    [EmailAddress(ErrorMessage = "Email should be valid")]
    public required string Email { get; init; }

    /// <summary>
    /// Password of the user.
    /// </summary>
    [Required]
    [RegularExpression(
        pattern: @"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password should contain uppercase and lowercase letters, digits, and special characters")]
    public required string Password { get; init; }
}
