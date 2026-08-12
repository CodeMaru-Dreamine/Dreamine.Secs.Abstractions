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

    [Fact]
    public void WireObservationIsDisabledByDefault()
    {
        var options = new HsmsSessionOptions { Mode = SecsConnectionMode.Active, Role = SecsRole.Host };

        options.Validate();

        Assert.Null(options.WireObservation);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(65_537, 1)]
    [InlineData(1_025, 65_536)]
    public void WireObservationBoundsQueueAndRetainedPayload(int queueCapacity, int maximumCapturedBytes)
    {
        var options = new HsmsWireObservationOptions
        {
            QueueCapacity = queueCapacity,
            MaximumCapturedBytes = maximumCapturedBytes
        };

        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public void WireObservationAcceptsExactlySixtyFourMebibytesOfRetainedPayload()
    {
        var wire = new HsmsWireObservationOptions
        {
            QueueCapacity = 1_024,
            MaximumCapturedBytes = 65_536
        };
        var options = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            WireObservation = wire
        };

        options.Validate();

        Assert.Equal(64L * 1024 * 1024, (long)wire.QueueCapacity * wire.MaximumCapturedBytes);
    }

    [Fact]
    public void WireObservationRetainedPayloadCalculationIsOverflowSafe()
    {
        var options = new HsmsWireObservationOptions
        {
            QueueCapacity = 65_536,
            MaximumCapturedBytes = int.MaxValue
        };

        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public void SessionValidationRejectsInvalidNestedWireObservation()
    {
        var options = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            WireObservation = new HsmsWireObservationOptions { QueueCapacity = 0 }
        };

        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public void WireObservationDefensivelyCopiesCallerMemory()
    {
        var bytes = new byte[14];
        bytes[3] = 10;
        var observation = new HsmsWireObservation(
            1,
            1,
            DateTimeOffset.UtcNow,
            HsmsWireDirection.Inbound,
            14,
            10,
            bytes);

        bytes[0] = 0xff;

        Assert.Equal(0, observation.CapturedBytes.Span[0]);
    }

    [Fact]
    public void WireCaptureRulesRejectDuplicatesAndLimitsBeyondTheGlobalBudget()
    {
        var duplicate = new HsmsWireObservationOptions
        {
            MaximumCapturedBytes = 1024,
            CaptureRules =
            [
                new HsmsWireCaptureRule(6, 11, null, HsmsWireCaptureMode.HeaderOnly),
                new HsmsWireCaptureRule(6, 11, null, HsmsWireCaptureMode.Excluded)
            ]
        };
        var oversized = new HsmsWireObservationOptions
        {
            MaximumCapturedBytes = 1024,
            CaptureRules =
            [
                new HsmsWireCaptureRule(6, 11, HsmsWireDirection.Inbound, HsmsWireCaptureMode.FullFrame, 1025)
            ]
        };

        Assert.Throws<ArgumentException>(duplicate.Validate);
        Assert.Throws<ArgumentOutOfRangeException>(oversized.Validate);
    }

    [Fact]
    public void WireObservationCanRetainTypedHeaderWithoutPayloadBytes()
    {
        var header = new HsmsHeader(7, 0x86, 11, 0, 0, new Dreamine.Secs.Abstractions.Model.SecsSystemBytes(3));
        var observation = new HsmsWireObservation(
            1,
            1,
            DateTimeOffset.UtcNow,
            HsmsWireDirection.Inbound,
            1024,
            1020,
            ReadOnlyMemory<byte>.Empty,
            header);

        Assert.Empty(observation.CapturedBytes.ToArray());
        Assert.Equal(header, observation.Header);
        Assert.True(observation.IsCaptureTruncated);
    }

    [Fact]
    public void SessionMessageLimitDefaultsToFrameTextCapacity()
    {
        var options = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            MaximumFrameLength = 10
        };

        options.Validate();

        Assert.Null(options.MaximumMessageLength);
    }

    [Theory]
    [InlineData(100, 91)]
    [InlineData(10, 1)]
    [InlineData(100, 0)]
    public void SessionRejectsMessageLimitOutsideFrameTextCapacity(int maximumFrameLength, int maximumMessageLength)
    {
        var options = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            MaximumFrameLength = maximumFrameLength,
            MaximumMessageLength = maximumMessageLength
        };

        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public void SessionAcceptsExplicitMessageLimitAtFrameTextBoundary()
    {
        var options = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            MaximumFrameLength = 100,
            MaximumMessageLength = 90
        };

        options.Validate();
    }

    [Theory]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    [InlineData(1, 16_777_216)]
    public void SessionRejectsInvalidItemStructureLimits(int maximumNestingDepth, int maximumListItemCount)
    {
        var options = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            MaximumNestingDepth = maximumNestingDepth,
            MaximumListItemCount = maximumListItemCount
        };

        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }
}
