namespace NyxScriptorium.Domain.Sessions;

/// <summary>
/// An audio file that belongs to a <see cref="SessionContext"/>.
/// Pure data: it holds only the name and never touches the filesystem.
/// </summary>
/// 
public record AudioFile
{
    public string FileName { get; }

    public AudioFile(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        FileName = fileName;
    }
}
