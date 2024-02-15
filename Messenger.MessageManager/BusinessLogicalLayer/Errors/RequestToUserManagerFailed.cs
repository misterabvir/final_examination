using Messenger.Shared.Abstractions.Results;

namespace Messenger.MessageManager.BusinessLogicalLayer.Errors;

public class RequestToUserManagerFailed : Error
{
    public override ErrorCode Code => ErrorCode.Conflict;

    public override string Message => "UserManager.NotAvailable";

    public override string? Description => "User Manager API not responding";
}
