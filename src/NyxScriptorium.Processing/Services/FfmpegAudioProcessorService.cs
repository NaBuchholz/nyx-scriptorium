using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Domain.Sessions;
using System.Diagnostics;

namespace NyxScriptorium.Processing.Services;

// TODO Implement external service 
public class FfmpegAudioProcessorService : IAudioProcessor
{
    public Task<AudioFile> PrepareAsync(AudioFile file, CancellationToken cancellationToken) => throw new NotImplementedException();
    public Task ValidateAsync(AudioFile file, CancellationToken cancellationToken)
    {
        if (!File.Exists(file.FilePath))
        {
            throw new AudioProcessingException("Audio file does not exist.");
        }
        try
        {
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
        }
        catch
        {

            throw;
        }
        return Task.CompletedTask;
    }
}
