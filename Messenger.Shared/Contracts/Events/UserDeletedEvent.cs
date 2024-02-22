namespace Messenger.Shared.Contracts.Events;

public record UserDeletedEvent
{
    public Guid Id { get; init; }
}
