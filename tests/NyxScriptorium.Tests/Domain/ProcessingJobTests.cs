using NUnit.Framework;
using NyxScriptorium.Domain;

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
    public void Setup()
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
    public void Job_Should_Be_Failed_When_Any_Step_Fails()
    {
        _validate.Start();
        _validate.Complete();
        _transcribe.Start();
        _transcribe.Fail();

        Assert.That(_job.Status, Is.EqualTo(JobStatus.Failed));
    }

    /// <summary>
    /// The job is Completed only when every step is completed: it is not Completed
    /// while one step is still pending, and becomes Completed once the last one finishes.
    /// </summary>
    [Test]
    public void JobShouldBeCompleted_OnlyWhen_AllStepsCompleted()
    {
        _validate.Start();
        _validate.Complete();
        _transcribe.Start();
        _transcribe.Complete();
        Assert.That(_job.Status, Is.Not.EqualTo(JobStatus.Completed));

        _generate.Start();
        _generate.Complete();
        Assert.That(_job.Status, Is.EqualTo(JobStatus.Completed));
    }
}
