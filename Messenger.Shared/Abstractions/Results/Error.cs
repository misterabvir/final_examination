namespace Messenger.Shared.Abstractions.Results;

/// <summary>
/// Represents an error.
/// </summary>
public abstract class Error
{
    /// <summary>
    /// Gets the error code.
    /// </summary>
    public abstract ErrorCode Code { get; }

    /// <summary>
    /// Gets the error message.
    /// </summary>
    public abstract string Message { get; }

    /// <summary>
    /// Gets the error description.
    /// </summary>
    public abstract string? Description { get; }
}