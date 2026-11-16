namespace NyxScriptorium.Domain.Sessions;

/// <summary>
/// An audio file that belongs to a <see cref="SessionContext"/>.
/// Stores its path and name without performing filesystem operations.
/// </summary>
public record AudioFile
{
    /// <summary>
    /// Gets the full path of the audio file.
    /// </summary>
    public string FilePath { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioFile"/> class.
    /// </summary>
    public AudioFile(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        FilePath = filePath;
    }
}
