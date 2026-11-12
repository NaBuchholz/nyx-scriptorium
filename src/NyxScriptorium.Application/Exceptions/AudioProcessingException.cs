namespace NyxScriptorium.Application.Exceptions;

/// <summary>
/// Represents an error that occurs while processing audio.
/// </summary>
public class AudioProcessingException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AudioProcessingException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that caused this error, if any.</param>
    public AudioProcessingException(string message, Exception? inner = null)
        : base(message, inner) { }
}
