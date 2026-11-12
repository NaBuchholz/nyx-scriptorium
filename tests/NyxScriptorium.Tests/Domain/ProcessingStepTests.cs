using FluentAssertions;
using NUnit.Framework;
using NyxScriptorium.Domain;

namespace NyxScriptorium.Tests.Domain;

/// <summary>
/// Tests for the state transition rules of <see cref="ProcessingStep"/>.
/// </summary>
[TestFixture]
public class ProcessingStepTests
{
    private ProcessingStep _step = null!;

    /// <summary>Creates a fresh step in the Pending state before each test.</summary>
    [SetUp]
    public void SetUp()
    {
        _step = new ProcessingStep("step-1");
    }

    /// <summary>
    /// Completed is a terminal state: a completed step cannot be started again,
    /// and the attempt is rejected with an <see cref="InvalidOperationException"/>.
    /// </summary>
    [Test]
        public void Start_ShouldThrowInvalidOperationException_WhenStepIsCompleted()
        {
            // Arrange
            _step.Start();
            _step.Complete();

           // Act
           Action act = () => _step.Start();
            
           // Assert
            act.Should().Throw<InvalidOperationException>();
        }
}
