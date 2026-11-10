namespace NyxScriptorium.Domain;

/// <summary>
/// Lifecycle state of a single <see cref="ProcessingStep"/>.
/// </summary>
public enum StepStatus
{
/// <summary>The step was created but has not started yet. Initial state.</summary>
Pending,
/// <summary>The step is currently being executed.</summary>
Running,
/// <summary>The step completed successfully.</summary>
Completed,
/// <summary>The step failed and could not be completed. Terminal state.</summary>
Failed,
/// <summary>The step was deliberately not executed (e.g. an optional step). Terminal state.</summary>
Skipped,
 /// <summary>The step was interrupted on request before finishing. Terminal state.</summary>
Cancelled
}
