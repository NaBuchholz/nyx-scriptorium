using NUnit.Framework;
using NyxScriptorium.Domain;

namespace NyxScriptorium.Tests.Domain;

/// <summary>
/// Tests for the initial state of <see cref="Session"/>.
/// </summary>
[TestFixture]
public class SessionTests
{
    private Session _session = null!;

    /// <summary>Creates a fresh session before each test.</summary>
    [SetUp]
    public void SetUp()
    {
        _session = new Session();
    }
    
    /// <summary>
    /// A session with no job attached has not started processing, so its status is Pending.
    /// </summary>
    [Test]
    public void SessionShouldStart_WithItsJobStatusAsPending()
    {
        Assert.That(_session.Status, Is.EqualTo(JobStatus.Pending));
    }
}
