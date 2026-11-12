using NUnit.Framework;
using NyxScriptorium.Domain;
using FluentAssertions;

namespace NyxScriptorium.Tests.Domain;

/// <summary>
/// Tests for the status that <see cref="ProcessingJob"/> derives from its steps.
/// </summary>
[TestFixture]
public class ProcessingJobTests
{
    private ProcessingStep _validate = null!;
    private ProcessingStep _transcribe = null!;
    private ProcessingStep _generate = null!;
    private ProcessingJob _job = null!;

    /// <summary>Creates a fresh job with three Pending steps before each test.</summary>
    [SetUp]
    public void SetUp()
    {
        _validate   = new ProcessingStep("ValidateFiles");
        _transcribe = new ProcessingStep("TranscribeAudio");
        _generate   = new ProcessingStep("GenerateArtifacts");
        _job = new ProcessingJob([_validate, _transcribe, _generate]);
    } 

    /// <summary>
    /// A single failed step is enough to make the whole job Failed,
    /// even when other steps have completed or have not run yet.
    /// </summary>
    [Test]
    public void Status_ShouldBeFailed_WhenAnyStepFails()
    {
        // Arrange        
        _validate.Start();
        _validate.Complete();

        // Act
        _transcribe.Start();
        _transcribe.Fail();

        // Assert
        _job.Status.Should().Be(JobStatus.Failed);
    }

    /// <summary>
    /// The job should not be Completed while any step is pending.
    /// </summary>
    [Test]
    public void Status_ShouldNotBeCompleted_WhenAnyStepIsPending()
    {
        // Arrange
        _validate.Start();
        _validate.Complete();

        _transcribe.Start();
        _transcribe.Complete();

        // Act
        var status = _job.Status;

        // Assert
        _job.Status.Should().NotBe(JobStatus.Completed);
    }

    /// <summary>
    /// The job should be Completed when all steps have completed.
    /// </summary>
    [Test]
    public void Status_ShouldBeCompleted_WhenAllStepsAreCompleted()
    {
        // Arrange
        _validate.Start();
        _validate.Complete();

        _transcribe.Start();
        _transcribe.Complete();

        // Act
        _generate.Start();
        _generate.Complete();

        var status = _job.Status;

        // Assert
        status.Should().Be(JobStatus.Completed);
    }
}
