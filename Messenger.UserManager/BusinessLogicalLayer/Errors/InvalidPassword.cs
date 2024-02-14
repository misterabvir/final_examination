using Messenger.Shared.Abstractions.Results;

namespace Messenger.UserManager.BusinessLogicalLayer.Errors;

public class InvalidPassword : Error
{
    public override ErrorCode Code => ErrorCode.BadRequest;

    public override string Message => $"User.InvalidPassword";

    public override string? Description => $"Invalid password";
}
