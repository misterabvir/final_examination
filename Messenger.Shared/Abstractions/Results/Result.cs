namespace Messenger.Shared.Abstractions.Results;

/// <summary>
/// Represents a result that can either be a success with a value of type <typeparamref name="T"/> or a failure with an error of type <typeparamref name="E"/>.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <typeparam name="E">The type of the error.</typeparam>

public class Result<T, E> where E : Error
{
    /// <summary>
    /// Gets or sets the value of the result.
    /// </summary>
    public T? Value { get; set; }

    /// <summary>
    /// Gets or sets the error of the result.
    /// </summary>
    public E? Error { get; set; }

    /// <summary>
    /// Indicates whether the result is a success.
    /// </summary>
    public bool IsSuccess => Error == null;

    private Result() { }

    /// <summary>
    /// Creates a success result with the specified value.
    /// </summary>
    /// <param name="value">The value of the result.</param>
    /// <returns>A success result with the specified value.</returns>
    public static Result<T, E> Success(T value) => new() { Value = value };

    /// <summary>
    /// Creates a failure result with the specified error.
    /// </summary>
    /// <param name="error">The error of the result.</param>
    /// <returns>A failure result with the specified error.</returns>
    public static Result<T, E> Failure(E error) => new() { Error = error };

    /// <summary>
    /// Implicitly converts a value to a success result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T, E>(T value) => Success(value);

    /// <summary>
    /// Implicitly converts an error to a failure result.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result<T, E>(E error) => Failure(error);
}



