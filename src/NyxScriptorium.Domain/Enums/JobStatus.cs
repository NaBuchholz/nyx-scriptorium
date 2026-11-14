using NyxScriptorium.Domain.Jobs;

namespace NyxScriptorium.Domain.Enums;

/// <summary>
/// Aggregate state of a <see cref="ProcessingJob"/>, derived from the states of its steps.
/// </summary>
public enum JobStatus
{
    /// <summary>No step has started yet.</summary>
    Pending,
    /// <summary>Processing is under way: at least one step has started and the job has not finished.</summary>
    Running,
    /// <summary>Every step is either completed or skipped.</summary>
    Completed,
    /// <summary>At least one step failed. Takes priority over <see cref="Cancelled"/>.</summary>
    Failed,
    /// <summary>At least one step was cancelled and none failed.</summary>
    Cancelled
}
