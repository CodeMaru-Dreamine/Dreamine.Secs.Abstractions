using Dreamine.Secs.Abstractions.Enums;
using Dreamine.Secs.Abstractions.Hsms;
using Xunit;

namespace Dreamine.Secs.Abstractions.Tests;

public sealed class HsmsOptionsTests
{
    [Fact]
    public void TimersRequireDocumentedOneSecondResolution()
    {
        var options = new HsmsTimerOptions { T8 = TimeSpan.FromMilliseconds(1500) };
        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Theory]
    [InlineData((SecsConnectionMode)999, SecsRole.Host)]
    [InlineData(SecsConnectionMode.Active, (SecsRole)999)]
    public void SessionOptionsRejectUndefinedRoleAndMode(SecsConnectionMode mode, SecsRole role)
    {
        var options = new HsmsSessionOptions { Mode = mode, Role = role };
        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public void AutomaticT5ReconnectIsLimitedToActiveMode()
    {
        var options = new HsmsSessionOptions { Mode = SecsConnectionMode.Passive, Role = SecsRole.Host, AutoReconnect = true };
        Assert.Throws<ArgumentException>(options.Validate);
    }
}
