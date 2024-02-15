namespace Messenger.Shared.Contracts.Messages.Responses;

public class MessageResponse
{
    public Guid SenderId { get; set; }
    public string Text { get; set; } = null!;
}
