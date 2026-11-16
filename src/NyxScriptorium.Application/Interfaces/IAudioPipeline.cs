using NyxScriptorium.Domain.Sessions;
using NyxScriptorium.Domain.Processing;

namespace NyxScriptorium.Application.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IAudioPipeline
{
    public Task<ProcessingResult> RunAsync(SessionContext session, CancellationToken cancellationToken);
}
