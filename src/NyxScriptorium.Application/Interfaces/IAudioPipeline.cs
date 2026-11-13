using NyxScriptorium.Domain;

namespace NyxScriptorium.Application.Interfaces;

public interface IAudioPipeline
{
    public Task<ProcessingJob> RunAsync(AudioFile file, CancellationToken cancellationToken);
}
