namespace Messenger.Shared.Security.Base;

/// <summary>
/// Interface defining the contract for token service.
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT token based on provided parameters.
    /// </summary>
    /// <param name="id">Unique identifier of the user</param>
    /// <param name="email">Email of the user</param>
    /// <param name="roleName">Role of the user (e.g. "Administrator", "User")</param>
    /// <returns></returns>
    string GenerateToken(Guid id, string email, string roleName);
}