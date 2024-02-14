using Messenger.Shared.Abstractions.Results;

namespace Messenger.UserManager.Errors;

public class UserCanNotDeleteThemSelves : Error
{
    public override ErrorCode Code => ErrorCode.Conflict;

    public override string Message => $"User.Delete";

    public override string? Description => "User can not delete themselves";
}