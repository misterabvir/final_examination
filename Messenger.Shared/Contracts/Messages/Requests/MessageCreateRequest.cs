using System.ComponentModel.DataAnnotations;

namespace Messenger.Shared.Contracts.Messages.Requests;

public class MessageCreateRequest
{
    [EmailAddress]
    public  required string RecipientEmail { get; set; }
    public string Text { get; set; } = null!;
}