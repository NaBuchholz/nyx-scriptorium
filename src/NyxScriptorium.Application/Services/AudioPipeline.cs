using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Domain;

namespace NyxScriptorium.Application.Services;

/// <summary>
/// A basic implementation of an audio processing pipeline.
/// </summary>
public class AudioPipeline(IAudioProcessor audioProcessor) : IAudioPipeline
{
    /// <inheritdoc />
    public async Task<ProcessingJob> RunAsync(AudioFile file, CancellationToken cancellationToken) 
    {
        var validateStep = new ProcessingStep("ValidateFile");
        var processingJob = new ProcessingJob([validateStep]);

        validateStep.Start();

        try
        {
            await audioProcessor.ValidateAsync(file, cancellationToken);
            validateStep.Complete();
        }
        catch (OperationCanceledException)
        {
            validateStep.Cancel();
            return processingJob;
        }
        catch
        {
            validateStep.Fail();
            return processingJob;
        }
        return processingJob;
    }
}
