using Moq;
using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Application.Services;
using NyxScriptorium.Domain.Enums;
using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Tests.Application;

/// <summary>
/// Tests the behavior of the <see cref="AudioPipeline"/> during audio validation.
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
    /// Verifies that the pipeline completes the validation step and the job
    /// when the audio file is successfully validated.
    /// </summary>
    [Test]
    public async Task RunAsync_ShouldCompleteValidation_WhenFileIsValid()
    {
        // Arrange
        _processor.Setup(x => x.ValidateAsync(
            _file,
            CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _sut.RunAsync(_session, CancellationToken.None);

        // Assert
        result.Session.Status.Should().Be(JobStatus.Completed);
        var validationStep = result.Session.Job!.Steps.Should().ContainSingle(
            x => x.Name == "ValidateFile").Subject;
        validationStep.Status.Should().Be(StepStatus.Completed);
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

    [Test]
    public async Task RunAsync_ShouldPrepareAudio_WhenValidationSucceeds() { }
    #endregion
}
