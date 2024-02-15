using Messenger.Shared.Abstractions.Results;
using Messenger.Shared.Contracts.Users.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Messenger.Shared.Abstractions.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected readonly ILogger _logger;

    protected BaseController(ILogger logger)
    {
        _logger = logger;
    }

    protected UserIdentity? GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        _logger.LogInformation("User identity: {identity}", identity);
        if (identity is null)
        {
            return null;
        }

        var id = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (id == null || email == null || role == null)
            return null;

        return new UserIdentity(Guid.Parse(id), email, role);
    }

    /// <summary>
    /// Generates an IActionResult based on the provided error code
    /// </summary>
    /// <param name="error">The error object containing the error code</param>
    /// <returns>An IActionResult based on the error code</returns>
    protected IActionResult ProblemActionResult(Error error)
    => error.Code switch
    {
        ErrorCode.NotFound => NotFound(error),
        ErrorCode.Conflict => Conflict(error),
        _ => BadRequest(error)
    };
}