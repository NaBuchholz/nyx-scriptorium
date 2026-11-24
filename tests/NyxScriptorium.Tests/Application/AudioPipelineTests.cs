using Moq;
using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Application.Services;
using NyxScriptorium.Domain.Enums;
using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Tests.Application;

/// <summary>
/// Tests the behavior of the <see cref="AudioPipeline"/> during audio processing.
/// </summary>
[TestFixture]
public class AudioPipelineTests
{
    private Mock<IAudioProcessor> _processor = null!;
    private IAudioPipeline _sut = null!;
    private AudioFile _file = null!;
    private SessionContext _session = null!;

    [SetUp]
    public void SetUp()
    {
        _processor = new Mock<IAudioProcessor>();
        _file = new AudioFile("test-audio.wav");

        _session = new SessionContext();
        _session.AddAudioFile(_file);

        _sut = new AudioPipeline(_processor.Object);

    }

    #region RunAsync Method
    /// <summary>
    /// Verifies that the pipeline completes all steps and the job
    /// when the processor succeeds.
    /// </summary>
    [Test]
    public async Task RunAsync_ShouldCompleteAllSteps_WhenProcessorSucceeds()
    {
        // Arrange
        var preparedFile = new AudioFile("prepared-audio.wav");

        _processor.Setup(x => x.ValidateAsync(
            _file,
            CancellationToken.None))
            .Returns(Task.CompletedTask);
        _processor.Setup(x => x.PrepareAsync(
            _file,
            CancellationToken.None))
            .ReturnsAsync(preparedFile);

        // Act
        var result = await _sut.RunAsync(_session, CancellationToken.None);

        // Assert
        result.Session.Status.Should().Be(JobStatus.Completed);
        var steps = result.Session.Job!.Steps;
        steps.Should().HaveCount(2);

        var validationStep = steps.Should().ContainSingle(
            x => x.Name == "ValidateFile").Subject;
        validationStep.Status.Should().Be(StepStatus.Completed);

        var preparationStep = steps.Should().ContainSingle(
            x => x.Name == "PrepareAudio").Subject;
        preparationStep.Status.Should().Be(StepStatus.Completed);

        result.Context!.PreparedAudio.Should().BeSameAs(preparedFile);
        _processor.VerifyAll();
    }

    /// <summary>
    /// Verifies that the pipeline marks the validation step and the job as failed
    /// when audio validation throws an <see cref="AudioProcessingException"/>.
    /// </summary>
    [Test]
    public async Task RunAsync_ShouldFailJob_WhenValidationThrowsAudioProcessingException()
    {
        // Arrange
        _processor.Setup(x => x.ValidateAsync(
            _file,
            CancellationToken.None))
            .ThrowsAsync(new AudioProcessingException("Invalid file"));

        // Act
        var result = await _sut.RunAsync(_session, CancellationToken.None);

        // Assert
        result.Session.Status.Should().Be(JobStatus.Failed);
        var validationStep = result.Session.Job!.Steps.Should().ContainSingle(
            x => x.Name == "ValidateFile").Subject;
        validationStep.Status.Should().Be(StepStatus.Failed);
        result.Session.Job.Steps.Should().NotContain(x => x.Name == "PrepareAudio");
        _processor.VerifyAll();
    }

    /// <summary>
    /// Verifies that the pipeline marks the validation step and the job as failed
    /// when validation throws an unexpected exception.
    /// </summary>
    [Test]
    public async Task RunAsync_ShouldFailJob_WhenValidationThrowsUnexpectedException()
    {
        // Arrange
        _processor.Setup(x => x.ValidateAsync(
            _file,
            CancellationToken.None))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await _sut.RunAsync(_session, CancellationToken.None);

        // Assert
        result.Session.Status.Should().Be(JobStatus.Failed);
        var validationStep = result.Session.Job!.Steps.Should().ContainSingle(
            x => x.Name == "ValidateFile").Subject;
        validationStep.Status.Should().Be(StepStatus.Failed);
        result.Session.Job.Steps.Should().NotContain(x => x.Name == "PrepareAudio");
        _processor.VerifyAll();
    }

    /// <summary>
    /// Verifies that the pipeline marks the validation step and the job as cancelled
    /// when validation is interrupted by an <see cref="OperationCanceledException"/>.
    /// </summary>
    [Test]
    public async Task RunAsync_ShouldCancelJob_WhenValidationIsCancelled()
    {
        // Arrange
        _processor.Setup(x => x.ValidateAsync(
            _file,
            CancellationToken.None))
            .ThrowsAsync(new OperationCanceledException());

        // Act
        var result = await _sut.RunAsync(_session, CancellationToken.None);

        // Assert
        result.Session.Status.Should().Be(JobStatus.Cancelled);
        var validationStep = result.Session.Job!.Steps.Should().ContainSingle(
            x => x.Name == "ValidateFile").Subject;
        validationStep.Status.Should().Be(StepStatus.Cancelled);
        _processor.VerifyAll();
    }

    /// <summary>
    /// Verifies that the pipeline fails the job when audio preparation throws.
    /// </summary>
    [Test]
    public async Task RunAsync_ShouldFailJob_WhenPreparationThrows()
    {
        // Arrange
        _processor.Setup(x => x.ValidateAsync(
            _file,
            CancellationToken.None))
            .Returns(Task.CompletedTask);
        _processor.Setup(x => x.PrepareAsync(
            _file,
            CancellationToken.None))
            .ThrowsAsync(new AudioProcessingException("Preparation failed"));

        // Act
        var result = await _sut.RunAsync(_session, CancellationToken.None);

        // Assert
        result.Session.Status.Should().Be(JobStatus.Failed);

        var validationStep = result.Session.Job!.Steps.Should().ContainSingle(
            x => x.Name == "ValidateFile").Subject;
        validationStep.Status.Should().Be(StepStatus.Completed);

        var preparationStep = result.Session.Job.Steps.Should().ContainSingle(
            x => x.Name == "PrepareAudio").Subject;
        preparationStep.Status.Should().Be(StepStatus.Failed);

        _processor.VerifyAll();
    }
    #endregion
}
