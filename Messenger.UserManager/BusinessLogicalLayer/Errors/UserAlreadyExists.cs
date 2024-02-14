using Messenger.Shared.Abstractions.Results;

namespace Messenger.UserManager.BusinessLogicalLayer.Errors;

public class UserAlreadyExists : Error
{
    public UserAlreadyExists(string email)
    {
        Description = $"User with email {email} already exists";
    }

    public override ErrorCode Code => ErrorCode.BadRequest;

    public override string Message => $"User.AlreadyExists";

    public override string? Description { get;  }
}