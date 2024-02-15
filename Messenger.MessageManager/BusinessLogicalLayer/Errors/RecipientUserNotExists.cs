using Messenger.Shared.Abstractions.Results;

namespace Messenger.MessageManager.BusinessLogicalLayer.Errors;

public class RecipientUserNotExists  : Error
{
    public override ErrorCode Code => ErrorCode.Conflict;

    public override string Message => "Message.Recipient";

    public override string? Description => "Recipient User not exists";
}