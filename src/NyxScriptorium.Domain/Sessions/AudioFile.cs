namespace NyxScriptorium.Domain;

/// <summary>
/// An audio file that belongs to a <see cref="Session"/>.
/// Pure data: it holds only the name and never touches the filesystem.
/// </summary>
/// <param name="FileName">The name of the audio file.</param>
public record AudioFile(string FileName)
{

}
