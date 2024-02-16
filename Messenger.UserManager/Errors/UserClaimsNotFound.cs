using Messenger.Shared.Abstractions.Results;
namespace Messenger.UserManager.Errors;

public class UserClaimsNotFound : Error
{
    public override ErrorCode Code => ErrorCode.Conflict;

    public override string Message => $"User.Claims";

    public override string? Description => "User claims not found";
}
