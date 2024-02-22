namespace Messenger.Shared.Contracts.Events;

public record UserRegisteredEvent
{
    public Guid Id { get; init; }
    public string Email { get; init; } = null!;
}
