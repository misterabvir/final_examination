namespace Messenger.UserManager.BusinessLogicalLayer.Services.Base;

/// <summary>
/// Interface for encryption-related operations.
/// </summary>
public interface IEncryptService
{
    /// <summary>
    /// Generates a salt for password hashing.
    /// </summary>
    /// <returns>A byte array representing the generated salt.</returns>
    byte[] GenerateSalt();

    /// <summary>
    /// Hashes the provided password using the specified salt.
    /// </summary>
    /// <param name="password">The password to hash.</param>
    /// <param name="salt">The salt used for hashing.</param>
    /// <returns>A byte array representing the hashed password.</returns>
    byte[] HashPassword(string password, byte[] salt);
}