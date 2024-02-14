using AutoMapper;
using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Users.Requests;
using Messenger.Shared.Contracts.Users.Responses;
using Messenger.Shared.Enums;
using Messenger.Shared.Security.Base;
using Messenger.UserManager.BusinessLogicalLayer.Errors;
using Messenger.UserManager.BusinessLogicalLayer.Services.Base;
using Messenger.UserManager.DataAccessLayer.Repositories.Base;
using Messenger.UserManager.Domain;
using Microsoft.IdentityModel.Tokens;

namespace Messenger.UserManager.BusinessLogicalLayer.Services;

/// <summary>
/// Class UserService. Implements the <see cref="IUserService" />.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IEncryptService _encryptService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the UserService class with the required dependencies.
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="roleRepository">The role repository</param>
    /// <param name="encryptService">The encrypt service</param>
    /// <param name="tokenService">The token service</param>
    /// <param name="mapper">The mapper</param>
    /// <param name="logger">The logger</param>
    public UserService(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IEncryptService encryptService,
        ITokenService tokenService,
        IMapper mapper,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _encryptService = encryptService;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }


    /// <summary>
    /// Retrieves all users
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>A collection of user responses or an error</returns>
    public async Task<Result<IEnumerable<UserResponse>, Error>> GetAll(CancellationToken cancellationToken)
    {
        // Retrieve all users from the repository
        var users = await _userRepository.GetAllAsync(cancellationToken);

        // Map the users to user responses and convert to list
        var response = users.Select(_mapper.Map<UserResponse>).ToList()!;

        // Log the action
        _logger.LogInformation("Retrieved all users");

        // Return the user responses
        return response;
    }

    /// <summary>
    /// Authenticates a user by email and password.
    /// </summary>
    /// <param name="request">The user authentication request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A result containing either a token or an error</returns>
    public async Task<Result<object, Error>> Login(UserAuthRequest request, CancellationToken cancellationToken)
    {
        // Retrieve user by email
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        // If user not found, return error
        if (user is null)
        {
            _logger.LogInformation("Login user failed: user email not found");
            return new UserNotFound(request.Email);
        }

        // Validate password
        var password = _encryptService.HashPassword(request.Password, user.Salt);
        if (!password.SequenceEqual(user.Password))
        {
            _logger.LogInformation("Login user failed: invalid password");
            return new InvalidPassword();
        }

        // Generate new salt and hash password
        user.Salt = _encryptService.GenerateSalt();
        user.Password = _encryptService.HashPassword(request.Password, user.Salt);
        await _userRepository.UpdateAsync(user, cancellationToken);

        // Generate token and log user login
        _logger.LogInformation("Login user");
        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Role!.ToString());
        return Result<object, Error>.Success(new { Token = token });
    }

    /// <summary>
    /// Registers a new user with the provided user authentication request.
    /// </summary>
    /// <param name="request">The user authentication request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The result of the registration process</returns>
    public async Task<Result<object, Error>> Register(UserAuthRequest request, CancellationToken cancellationToken)
    {
        // Check if the user already exists
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (user is not null)
        {
            _logger.LogInformation("Register user failed: user email already exists");
            return new UserAlreadyExists(request.Email);
        }

        // Get all users and determine the default role
        var users = await _userRepository.GetAllAsync(cancellationToken);
        var role = await _roleRepository.GetRoleByRoleTypeAsync(users.IsNullOrEmpty() ? RoleType.Administrator : RoleType.User, cancellationToken)
            ?? throw new NotImplementedException();

        // Generate salt and hash the password
        var salt = _encryptService.GenerateSalt();
        var password = _encryptService.HashPassword(request.Password, salt);

        // Create a new user and store it in the repository
        user = new User
        {
            Email = request.Email,
            Password = password,
            Salt = salt,
            Role = role,
        };

        await _userRepository.CreateAsync(user, cancellationToken);

        _logger.LogInformation("Register user");

        // Generate and return a token for the registered user
        var token = _tokenService.GenerateToken(user.Id, user.Email, user.Role!.ToString());
        return Result<object, Error>.Success(new { Token = token });
    }

    /// <summary>
    /// Deletes a user based on the provided request
    /// </summary>
    /// <param name="request">The request containing the user ID to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A task representing the operation result</returns>
    public async Task<Result<object, Error>> Delete(UserDeleteRequest request, CancellationToken cancellationToken)
    {
        // Get the user by ID
        var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
        if (user is null)
        {
            // Return error if user is not found
            return new UserNotFound(request.Id);
        }

        // Delete the user
        await _userRepository.DeleteAsync(user!, cancellationToken);

        // Log the user deletion
        _logger.LogInformation("Delete user");

        // Return success result
        return Result<object, Error>.Success(new { Success = true });
    }
}
