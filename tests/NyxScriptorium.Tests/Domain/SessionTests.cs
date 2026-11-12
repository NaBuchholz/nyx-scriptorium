using FluentAssertions;
using NUnit.Framework;
using NyxScriptorium.Domain;

namespace NyxScriptorium.Tests.Domain;

/// <summary>
/// Tests for the initial state of <see cref="SessionContext"/>.
/// </summary>
[TestFixture]
public class SessionTests
{
    private SessionContext _session = null!;

    /// <summary>Creates a fresh SessionContext before each test.</summary>
    [SetUp]
    public void SetUp()
    {
        _session = new SessionContext();
    }
    
    /// <summary>
    /// A SessionContext with no job attached has not started processing, so its status is Pending.
    /// </summary>
    [Test]
    public void Session_ShouldBePending_WhenSessionHasNoJob()
    {
        // Act
        var status = _session.Status;

        // Assert
        status.Should().Be(JobStatus.Pending);
    }
}
