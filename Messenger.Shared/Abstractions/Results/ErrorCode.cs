namespace Messenger.Shared.Abstractions.Results;

/// <summary>
/// Represents error codes.
/// </summary>
public enum ErrorCode
{
    /// <summary>
    /// Indicates a bad request error.
    /// </summary>
    BadRequest = 400,

    /// <summary>
    /// Indicates a not found error.
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Indicates a conflict error.
    /// </summary>
    Conflict = 409,
}