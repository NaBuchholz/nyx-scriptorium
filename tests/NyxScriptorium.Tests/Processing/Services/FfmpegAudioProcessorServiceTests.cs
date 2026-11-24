using NyxScriptorium.Application.Exceptions;
using NyxScriptorium.Domain.Sessions;
using NyxScriptorium.Processing.Services;

namespace NyxScriptorium.Tests.Processing.Services;

public class FfmpegAudioProcessorServiceTests
{
    private AudioFile _file = null!;
    private FfmpegAudioProcessorService _sut = null!;

    [SetUp]
    public void Setup()
    {
        _sut = new FfmpegAudioProcessorService();
        _file = new AudioFile(Path.Combine(
            TestContext.CurrentContext.TestDirectory,
            "TestData",
            "Audio",
            "valid-short.mp3"));
    }

    [Test]
    public void ValidateAsync_ShouldFail_WhenFileDoesNotExist()
    {
        // Arrange
        var missingFile = new AudioFile(Path.Combine(TestContext.CurrentContext.TestDirectory, "missing-audio.wav"));

        // Act
        var exception = Assert.ThrowsAsync<AudioProcessingException>(
            () => _sut.ValidateAsync(
                missingFile,
                CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
    }

    [Test]
    public async Task ValidateAsync_ShouldComplete_WhenFileIsValid()
    {
        // Arrange
        Func<Task> act = () => _sut.ValidateAsync(
            _file,
            CancellationToken.None);

        // Act
        var assertion = act.Should().NotThrowAsync();

        // Assert
        await assertion;
    }

    [Test]
    [Category("Integration")]
    public void ValidateAsync_ShouldFail_WhenFileIsNotAudio()
    {
        try
        {
            // Arrange
            // Act
            // Assert
        }
        finally
        {

        }
    }

    [Test]
    [Category("Integration")]
    public void PrepareAsync_ShouldFail_WhenFfmpegReturnsError()
    {
        try
        {
            // Arrange
            // Act
            // Assert
        }
        finally
        {
            // if (File.Exists(outputPath))
            //{
            //    File.Delete(outputPath);
            //}
        }
    }
}
