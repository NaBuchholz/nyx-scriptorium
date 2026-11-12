using System;

namespace NyxScriptorium.Domain;

/// <summary>
/// One independent stage of a processing pipeline (e.g. validating files, transcribing audio).
/// Owns its own state and only allows valid transitions between <see cref="StepStatus"/> values.
/// </summary>
public class ProcessingStep
{
    /// <summary>
    /// Single source of truth for the transition rules: for each state, the states it may move to.
    /// Terminal states have no allowed destination.
    /// </summary>
    private static readonly IReadOnlyDictionary<StepStatus, StepStatus[]> AllowedTransitions =
        new Dictionary<StepStatus, StepStatus[]>
        {
            [StepStatus.Pending]   = [StepStatus.Running, StepStatus.Skipped, StepStatus.Cancelled],
            [StepStatus.Running]   = [StepStatus.Completed, StepStatus.Failed, StepStatus.Cancelled],
            [StepStatus.Completed] = [],
            [StepStatus.Failed]    = [],
            [StepStatus.Skipped]   = [],
            [StepStatus.Cancelled] = [],
        };

    /// <summary>The name of the step.Starts of <see cref="Status.Pending"/> .</summary>
    public string Name { get; }

    /// <summary>
    /// The current state of the step. Starts as <see cref="StepStatus.Pending"/> .</summary>
    public StepStatus Status { get; private set; } = StepStatus.Pending;

    /// <summary>Creates a new step in the <see cref="StepStatus.Pending"/> state.</summary>
    /// <param name="name">The name of the step.</param>
    /// <exception cref="ArgumentException"><paramref name="name"/> is null, empty or whitespace.</exception>
    public ProcessingStep(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
    }

    /// <summary>Moves the step from <see cref="StepStatus.Pending"/> to <see cref="StepStatus.Running"/>.</summary>
    /// <exception cref="InvalidOperationException">The transition is not allowed from the current state.</exception>
    public void Start()    => TransitionTo(StepStatus.Running);

    /// <summary>Moves the step from <see cref="StepStatus.Running"/> to <see cref="StepStatus.Completed"/>.</summary>
    /// <exception cref="InvalidOperationException">The transition is not allowed from the current state.</exception>
    public void Complete() => TransitionTo(StepStatus.Completed);

    /// <summary>Moves the step from <see cref="StepStatus.Running"/> to <see cref="StepStatus.Failed"/>.</summary>
    /// <exception cref="InvalidOperationException">The transition is not allowed from the current state.</exception>
    public void Fail()     => TransitionTo(StepStatus.Failed);

    /// <summary>Moves the step from <see cref="StepStatus.Pending"/> to <see cref="StepStatus.Skipped"/>.</summary>
    /// <exception cref="InvalidOperationException">The transition is not allowed from the current state.</exception>
    public void Skip()     => TransitionTo(StepStatus.Skipped);

    /// <summary>
    /// Moves the step from <see cref="StepStatus.Pending"/> or <see cref="StepStatus.Running"/>
    /// to <see cref="StepStatus.Cancelled"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">The transition is not allowed from the current state.</exception>
    public void Cancel()   => TransitionTo(StepStatus.Cancelled);

    /// <summary>
    /// The only place that changes <see cref="Status"/>. Checks the transition rules
    /// and leaves the state untouched when the move is rejected.
    /// </summary>
    /// <param name="next">The desired new state.</param>
    /// <exception cref="InvalidOperationException">The transition is not allowed from the current state.</exception>
    private void TransitionTo(StepStatus next)
    {
        if (!AllowedTransitions[Status].Contains(next))
            throw new InvalidOperationException(
                $"Step '{Name}' cannot go from {Status} to {next}.");

        Status = next;
    }
}
