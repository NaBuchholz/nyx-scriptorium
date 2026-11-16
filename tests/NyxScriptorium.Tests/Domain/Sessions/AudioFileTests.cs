using NyxScriptorium.Domain.Sessions;

namespace NyxScriptorium.Tests.Domain.Sessions;

/// <summary>
/// Tests the behavior of the <see cref="AudioFile"/> domain model.
/// </summary>
[TestFixture]
public class AudioFileTests
{
    private string _filePath = null!;

    [SetUp]
    public void SetUp()
    {
        _filePath = "/home/user/audio/session.wav";
    }

    /// <summary>
    /// Verifies that the audio file stores the provided path.
    /// </summary>
    [Test]
    public void Constructor_ShouldStoreFilePath_WhenPathIsValid()
    {
        // Act
        var audioFile = new AudioFile(_filePath);

        // Assert
        audioFile.FilePath.Should().Be(_filePath);
    }

    /// <summary>
    /// Verifies that the constructor rejects a null file path.
    /// </summary>
    [Test]
    public void Constructor_ShouldThrowArgumentNullException_WhenFilePathIsNull()
    {
        // Arrange
        string? filePath = null;

        // Act
        var result = () => new AudioFile(filePath!);

        // Assert
        result.Should().Throw<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that the constructor rejects an empty file path.
    /// </summary>
    [Test]
    public void Constructor_ShouldThrowArgumentException_WhenFilePathIsEmpty()
    {
        // Arrange
        var filePath = string.Empty;

        // Act
        var result = () => new AudioFile(filePath);

        // Assert
        result.Should().Throw<ArgumentException>();
    }
}
