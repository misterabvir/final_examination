using Messenger.Shared.Abstractions.Models;

namespace Messenger.MessageManager.DataAccessLayer.Models;

public class AvailableUser : Entity
{
    public required string Email { get; set; }
}