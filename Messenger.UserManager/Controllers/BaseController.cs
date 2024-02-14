using Messenger.Shared.Abstractions.Results;
using Messenger.UserManager.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Messenger.UserManager.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    protected UserIdentityModel? GetCurrentUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        if (identity is null)
        {
            return null;
        }

        var id = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        var email = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var role = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

        if (id == null || email == null || role == null)
            return null;

        return new UserIdentityModel(Guid.Parse(id), email, role);
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