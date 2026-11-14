using System;
using NyxScriptorium.Domain.Enums;

namespace NyxScriptorium.Domain.Jobs;

/// <summary>
/// A full processing run, made of an ordered list of <see cref="ProcessingStep"/>.
/// It does not store its own state: <see cref="Status"/> is derived from the steps.
/// </summary>
public class ProcessingJob
{
    private readonly List<ProcessingStep> _steps;

    /// <summary>The steps of the job, in order. Read-only for callers.</summary>
    public IReadOnlyList<ProcessingStep> Steps => _steps;

    /// <summary>Creates a job from the given steps.</summary>
    /// <param name="steps">The steps that make up the job. Must contain at least one step.</param>
    /// <exception cref="ArgumentNullException"><paramref name="steps"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="steps"/> is empty.</exception>
    public ProcessingJob(IEnumerable<ProcessingStep> steps)
    {
        ArgumentNullException.ThrowIfNull(steps);

        _steps = steps.ToList();

        if (_steps.Count == 0)
            throw new ArgumentException("A job must have at least one step.", nameof(steps));
    }

    /// <summary>
    /// The aggregate state, recalculated on every read so it never goes stale.
    /// Priority: any failed step gives <see cref="JobStatus.Failed"/>; otherwise any cancelled step
    /// gives <see cref="JobStatus.Cancelled"/>; otherwise all completed or skipped gives
    /// <see cref="JobStatus.Completed"/>; otherwise all pending gives <see cref="JobStatus.Pending"/>;
    /// anything else is <see cref="JobStatus.Running"/>.
    /// </summary>
    public JobStatus Status
    {
        get
        {
            if (_steps.Any(s => s.Status == StepStatus.Failed))
                return JobStatus.Failed;

            if (_steps.Any(s => s.Status == StepStatus.Cancelled))
                return JobStatus.Cancelled;

            if (_steps.All(s => s.Status is StepStatus.Completed or StepStatus.Skipped))
                return JobStatus.Completed;

            if (_steps.All(s => s.Status == StepStatus.Pending))
                return JobStatus.Pending;

            return JobStatus.Running;
        }
    }
}
