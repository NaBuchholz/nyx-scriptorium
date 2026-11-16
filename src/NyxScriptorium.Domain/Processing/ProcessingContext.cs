using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Domain.Processing;

public record class ProcessingContext(AudioFile SourceAudio)
{
    public AudioFile? PreparedAudio { get; set; }
}
