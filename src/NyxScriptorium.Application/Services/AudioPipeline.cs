using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Domain.Jobs;
using NyxScriptorium.Domain.Processing;
using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Application.Services;

/// <summary>
/// A basic implementation of an audio processing pipeline.
/// </summary>
public class AudioPipeline(IAudioProcessor audioProcessor) : IAudioPipeline
{
    /// <inheritdoc />
    public async Task<ProcessingResult> RunAsync(SessionContext sessionContext, CancellationToken cancellationToken)
    {
        var validateStep = new ProcessingStep("ValidateFile");
        var processingJob = new ProcessingJob([validateStep]);
        ProcessingContext? processingContext = null;

        try
        {
            sessionContext.AttachJob(processingJob);

            var sourceAudio = await ValidateFileAsync(validateStep, sessionContext, cancellationToken);
            processingContext = new ProcessingContext(sourceAudio);

            var prepareAudioStep = new ProcessingStep("PrepareAudio");
            processingJob.AddStep(prepareAudioStep);

            processingContext.PreparedAudio = await PrepareAudioAsync(
                prepareAudioStep, sourceAudio, cancellationToken);
        }
        catch
        {
        }
        return new ProcessingResult(
            Session: sessionContext,
            Context: processingContext);
    }

    private async Task ExecuteStepAsync(
        ProcessingStep step,
        Func<Task> operation)
    {
        step.Start();

        try
        {
            await operation();
            step.Complete();
        }
        catch (OperationCanceledException)
        {
            step.Cancel();
            throw;
        }
        catch
        {
            step.Fail();
            throw;
        }
    }

    private async Task<T> ExecuteStepAsync<T>(
        ProcessingStep step,
        Func<Task<T>> operation)
    {
        step.Start();

        try
        {
            var result = await operation();
            step.Complete();

            return result;
        }
        catch (OperationCanceledException)
        {
            step.Cancel();
            throw;
        }
        catch
        {
            step.Fail();
            throw;
        }
    }

    private Task<AudioFile> ValidateFileAsync(
        ProcessingStep step,
        SessionContext session,
        CancellationToken cancellationToken)
    {
        return ExecuteStepAsync(step, async () =>
        {
            var file = GetSingleAudioFile(session);
            await audioProcessor.ValidateAsync(file, cancellationToken);
            return file;
        });
    }

    private Task<AudioFile> PrepareAudioAsync(
        ProcessingStep step,
        AudioFile file,
        CancellationToken cancellationToken)
    {
        return ExecuteStepAsync(
            step,
            () => audioProcessor.PrepareAsync(file, cancellationToken));
    }

    // TODO: Support multiple audio files and/or different formats
    private static AudioFile GetSingleAudioFile(SessionContext session)
    {
        if (session.AudioFiles.Count != 1)
            throw new AudioProcessingException("Only one audio file is currently supported.");

        return session.AudioFiles.First();
    }
}
