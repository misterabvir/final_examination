namespace Messenger.Shared.Contracts.Messages.Requests;

public class MessageCreateRequest
{
    public Guid RecipientId { get; set; }
    public string Text { get; set; } = null!;
}