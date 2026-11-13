using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Application.Interfaces;

/// <summary>Validates and prepares audio files for processing.</summary>
public interface IAudioProcessor
{
    /// <summary>Checks that the file can be processed.</summary>
    /// <exception cref="AudioProcessingException">The file is missing or not valid audio.</exception>
    Task ValidateAsync(AudioFile file, CancellationToken cancellationToken);

    /// <summary>Converts the file into the format the transcription expects.</summary>
    /// <returns>The prepared audio file.</returns>
    /// <exception cref="AudioProcessingException">The preparation failed.</exception>
    Task<AudioFile> PrepareAsync(AudioFile file, CancellationToken cancellationToken);
}
