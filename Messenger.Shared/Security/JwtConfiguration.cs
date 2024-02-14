using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.Shared.Security;
/// <summary>
/// Represents the configuration settings for JWT tokens.
/// </summary>
public class JwtConfiguration
{
    // File paths for public and private keys
    private const string PUBLICKEYFILEPATH = "./rsa/public_key.pem";
    private const string PRIVATEKEYFILEPATH = "./rsa/private_key.pem";

    /// <summary>
    /// Gets or sets the issuer of the JWT token.
    /// </summary>
    public required string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the audience of the JWT token.
    /// </summary>
    public required string Audience { get; set; }

    /// <summary>
    /// Gets or sets the expiration time of the JWT token in minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; }

    private RsaSecurityKey? _publicKey;
    private RsaSecurityKey? _privateKey;

    /// <summary>
    /// Gets the public key used for token verification.
    /// </summary>
    public RsaSecurityKey PublicKey
    {
        get
        {
            if (_publicKey is null)
            {
                EnsurePublicKeyExists();
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText(PUBLICKEYFILEPATH));
                _publicKey = new RsaSecurityKey(rsa);
            }

            return _publicKey;
        }
    }

    /// <summary>
    /// Gets the private key used for token signing.
    /// </summary>
    public RsaSecurityKey PrivateKey
    {
        get
        {
            if (_privateKey is null)
            {
                EnsurePrivateKeyExists(); // Check if private key file exists
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText(PRIVATEKEYFILEPATH)); // Import private key from file
                _privateKey = new RsaSecurityKey(rsa);
            }
            return _privateKey;
        }
    }

    /// <summary>
    /// Ensures that the public key file exists; throws FileNotFoundException if not found
    /// </summary>
    /// <exception cref="FileNotFoundException"></exception>
    private static void EnsurePublicKeyExists()
    {
        if (!File.Exists(PUBLICKEYFILEPATH))
        {
            throw new FileNotFoundException("Public key file not found.", PUBLICKEYFILEPATH);
        }
    }

    /// <summary>
    /// Ensures that the private key file exists; throws FileNotFoundException if not found
    /// </summary>
    /// <exception cref="FileNotFoundException"></exception>
    private static void EnsurePrivateKeyExists()
    {
        if (!File.Exists(PRIVATEKEYFILEPATH))
        {
            throw new FileNotFoundException("Private key file not found.", PRIVATEKEYFILEPATH);
        }
    }
}