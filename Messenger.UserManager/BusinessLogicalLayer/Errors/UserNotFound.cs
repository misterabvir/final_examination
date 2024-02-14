using Messenger.Shared.Abstractions.Results;

namespace Messenger.UserManager.BusinessLogicalLayer.Errors;

public class UserNotFound : Error
{
    public UserNotFound(Guid id)
    {
        Description = $"User with id {id} not found";
    }

    public UserNotFound(string email)
    {
        Description = $"User with email {email} not found";
    }

    public override ErrorCode Code => ErrorCode.NotFound;

    public override string Message => $"User.NotFound";

    public override string? Description { get; }
}
