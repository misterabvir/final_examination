using Messenger.Shared.Contracts.Users.Requests;
using Messenger.UserManager.BusinessLogicalLayer.Services.Base;
using Messenger.UserManager.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.UserManager.Controllers;

[Route("users")]
public class UsersController : BaseController
{
    private readonly IUserService _userService;

    /// <summary>
    /// Constructor for UsersController class, takes an instance of IUserService as a parameter.
    /// </summary>
    /// <param name="userService"></param>
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Gets all users.
    /// </summary>
    /// <remarks>
    /// Requires administrator or user role.
    /// </remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>Returns a list of all users.</returns>
    [Authorize(Roles = "Administrator, User")]
    [HttpGet(template: "get-all")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        // Call the UserService to get all users
        var response = await _userService.GetAll(cancellationToken);
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        // Return problem action result if there was an error
        return ProblemActionResult(response.Error!);
    }

    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="request">The user authentication request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>ActionResult with the result of the registration</returns>
    [AllowAnonymous]
    [HttpPost(template: "register")]
    public async Task<IActionResult> Register([FromBody] UserAuthRequest request, CancellationToken cancellationToken)
    {
        // Call the UserService to register the user
        var response = await _userService.Register(request, cancellationToken);

        // If the registration is successful, return the user data
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        // If there's an error with the registration, return an error result
        return ProblemActionResult(response.Error!);
    }

    /// <summary>
    /// Get the current user's ID
    /// </summary>
    [Authorize(Roles = "Administrator, User")]
    [HttpGet(template: "get-current-user-id")]
    public IActionResult GetCurrentUserId()
    {
        // Get the current user
        var user = GetCurrentUser();

        // If user is not found, return a problem
        if (user == null)
        {
            return ProblemActionResult(new UserClaimsNotFound());
        }

        // Return the current user's ID
        return Ok(new { UserId = user.Id });
    }

    /// <summary>
    /// Endpoint for user login
    /// </summary>
    [AllowAnonymous]
    [HttpPost(template: "login")]
    public async Task<IActionResult> Login([FromBody] UserAuthRequest request, CancellationToken cancellationToken)
    {
        // Call the user service to handle the login request
        var response = await _userService.Login(request, cancellationToken);

        // If the login is successful, return the user data
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        // If there was an error during login, return a problem response
        return ProblemActionResult(response.Error!);
    }

    /// <summary>
    /// Endpoint for deleting a user.
    /// </summary>
    /// <param name="request">The request containing the user id to be deleted.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>An IActionResult representing the result of the delete operation.</returns>
    [Authorize(Roles = "Administrator")]
    [HttpPost(template: "delete")]
    public async Task<IActionResult> Delete([FromBody] UserDeleteRequest request, CancellationToken cancellationToken)
    {
        // Get the current user
        var currentUser = GetCurrentUser();

        // If current user is not found, return a problem action result with UserClaimsNotFound error
        if (currentUser == null)
        {
            return ProblemActionResult(new UserClaimsNotFound());
        }

        // If current user is trying to delete themselves, return a problem action result with UserCanNotDeleteThemSelves error
        if (currentUser.Id == request.Id)
        {
            return ProblemActionResult(new UserCanNotDeleteThemSelves());
        }

        // Delete the user using the userService
        var response = await _userService.Delete(request, cancellationToken);

        // If the delete operation is successful, return Ok with the response value
        if (response.IsSuccess)
        {
            return Ok(response.Value);
        }

        // If the delete operation is not successful, return a problem action result with the response error
        return ProblemActionResult(response.Error!);
    }
}
