using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Domain.Sessions;
using System.ComponentModel;
using System.Diagnostics;

namespace NyxScriptorium.Processing.Services;

// TODO Implement external service 
public class FfmpegAudioProcessorService : IAudioProcessor
{
    public Task<AudioFile> PrepareAsync(AudioFile file, CancellationToken cancellationToken) => throw new NotImplementedException();

    /// <inheritdoc />
    /// <remarks>
    /// Uses the FFmpeg executable available on the system PATH to decode the input
    /// without producing a converted output file.
    /// </remarks>
    /// <exception cref="AudioProcessingException">FFmpeg could not be started.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled.</exception>
    public async Task ValidateAsync(AudioFile file, CancellationToken cancellationToken)
    {
        if (!File.Exists(file.FilePath))
        {
            throw new AudioProcessingException("Audio file does not exist.");
        }

        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "ffmpeg",
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            ArgumentList =
                {
                    "-hide_banner",
                    "-loglevel",
                    "error",
                    "-nostdin",
                    "-i",
                    file.FilePath,
                    "-f",
                    "null",
                    "-"
                }
        };

        using var process = new Process
        {
            StartInfo = processStartInfo
        };

        try
        {
            if (!process.Start())
            {
                throw new AudioProcessingException("FFmpeg not initialized properly.");
            }
        }
        catch (Win32Exception exception)
        {
            throw new AudioProcessingException(
                "Could not start FFmpeg. Ensure it is installed and available on PATH.",
                exception);
        }

        var errorTask = process.StandardError.ReadToEndAsync();
        var outputTask = process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync(cancellationToken);

        string standardError = await errorTask;
        _ = await outputTask;

        if (process.ExitCode != 0)
        {
            throw new AudioProcessingException(
                $"FFmpeg error: {standardError.Trim()}");
        }
    }
}

