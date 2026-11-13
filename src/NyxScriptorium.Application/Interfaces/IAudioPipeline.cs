using NyxScriptorium.Domain.Sessions;
using NyxScriptorium.Domain.Jobs;

namespace NyxScriptorium.Application.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IAudioPipeline
{
    public Task<ProcessingJob> RunAsync(AudioFile file, CancellationToken cancellationToken);
}
