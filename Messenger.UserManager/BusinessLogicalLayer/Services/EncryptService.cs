using Messenger.UserManager.BusinessLogicalLayer.Services.Base;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Messenger.UserManager.BusinessLogicalLayer.Services;

/// <summary>
/// Provides methods for generating salt and hashing passwords.
/// </summary>
public class EncryptService : IEncryptService
{
    /// <summary>
    /// Generates a random salt.
    /// </summary>
    /// <returns>A byte array representing the generated salt.</returns>
    public byte[] GenerateSalt() => Guid.NewGuid().ToByteArray();

    /// <summary>
    /// Hashes the given password using PBKDF2 algorithm with HMAC-SHA512.
    /// </summary>
    /// <param name="password">The password to be hashed.</param>
    /// <param name="salt">The salt used in hashing.</param>
    /// <returns>A byte array representing the hashed password.</returns>
    public byte[] HashPassword(string password, byte[] salt) =>
        KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA512,
            iterationCount: 10000,
            numBytesRequested: 512 / 8);
}