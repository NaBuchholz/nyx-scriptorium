using Moq;
using NyxScriptorium.Application.Interfaces;
using NyxScriptorium.Application.Services;

namespace NyxScriptorium.Tests.Application;

[TestFixture]
public class AudioPipelineTests
{
    private Mock<IAudioProcessor> _processor = null!;
    private Mock<IAudioPipeline> _pipeline = null!;

    [SetUp]
    public void SetUp()
    {
        _processor = new Mock<IAudioProcessor>();
        _pipeline = new Mock<IAudioPipeline>();
    }
    // TODO create test 
    [Test]
    public void Test1()
    {
        // Arrange
        // Act
        // Assert
    }
}
