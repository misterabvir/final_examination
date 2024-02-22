namespace Messenger.Shared.Contracts.Messages.Responses;

public class MessageResponse
{
    public required string Sender{ get; set; }
    public string Text { get; set; } = null!;
}
