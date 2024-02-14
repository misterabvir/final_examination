using Messenger.Shared.Security.Base;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Messenger.Shared.Security;

/// <summary>
/// TokenService class implements the ITokenService interface and is responsible for generating JWT tokens.
/// </summary>
internal class TokenService : ITokenService
{
   /// <summary>
   /// JwtConfiguration holds the configuration settings related to JWT tokens.
   /// </summary>
    private readonly JwtConfiguration _jwtConfiguration;

    /// <summary>
    /// Constructor injection to initialize TokenService with JwtConfiguration.
    /// </summary>
    /// <param name="jwtConfiguration"></param>
    public TokenService(JwtConfiguration jwtConfiguration)
    {
        _jwtConfiguration = jwtConfiguration;
    }

    /// <summary>
    /// Generates a JWT token based on provided parameters.
    /// </summary>
    /// <param name="id">Unique identifier of the user</param>
    /// <param name="email">Email of the user</param>
    /// <param name="roleName">Role of the user (e.g. "Administrator", "User")</param>
    /// <returns></returns>
    public string GenerateToken(Guid id, string email, string roleName)
    {
        // Define signing credentials using private key and algorithm.
        var credentials = new SigningCredentials(
            _jwtConfiguration.PrivateKey,
            SecurityAlgorithms.RsaSha256);

        // Define claims for the token including user identifier, email, and role.
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, id.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, roleName)
        };

        // Create a new JWT token with specified claims, issuer, audience, expiration time, and signing credentials.
        var token = new JwtSecurityToken(
            issuer: _jwtConfiguration.Issuer,
            audience: _jwtConfiguration.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(_jwtConfiguration.ExpirationMinutes),
            signingCredentials: credentials);

        // Write the token as a string.
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
