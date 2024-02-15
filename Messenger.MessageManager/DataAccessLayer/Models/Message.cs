using Messenger.Shared.Abstractions.Models;

namespace Messenger.MessageManager.DataAccessLayer.Models;

public class Message : Entity
{
    public required string Text { get; set; }
    public required Guid SenderId { get; set; }
    public required Guid RecipientId { get; set; }
    public bool IsRead { get; set; } = false;
}
