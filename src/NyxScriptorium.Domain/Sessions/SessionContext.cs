using System;

namespace NyxScriptorium.Domain;

/// <summary>
/// A recording session: one or more audio files and, once processing is requested,
/// the <see cref="ProcessingJob"/> that handles them.
/// </summary>
public class SessionContext
{
    private readonly List<AudioFile> _audioFiles = [];

    /// <summary>Unique identifier of the session.</summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>The audio files of the session. Read-only for callers.</summary>
    public IReadOnlyList<AudioFile> AudioFiles => _audioFiles;

    /// <summary>The processing job attached to this session, or null if processing has not been set up yet.</summary>
    public ProcessingJob? Job { get; private set; }

    /// <summary>
    /// The state of the session: <see cref="JobStatus.Pending"/> while there is no job,
    /// otherwise the status of the attached <see cref="Job"/>.
    /// </summary>
    public JobStatus Status => Job?.Status ?? JobStatus.Pending;

    /// <summary>Adds an audio file to the session.</summary>
    /// <param name="file">The file to add.</param>
    /// <exception cref="ArgumentNullException"><paramref name="file"/> is null.</exception>
    public void AddAudioFile(AudioFile file)
    {
        ArgumentNullException.ThrowIfNull(file);
        _audioFiles.Add(file);
    }

    /// <summary>Attaches the processing job that will handle this session.</summary>
    /// <param name="job">The job to attach.</param>
    /// <exception cref="ArgumentNullException"><paramref name="job"/> is null.</exception>
    public void AttachJob(ProcessingJob job)
    {
        ArgumentNullException.ThrowIfNull(job);
        Job = job;
    }
}
