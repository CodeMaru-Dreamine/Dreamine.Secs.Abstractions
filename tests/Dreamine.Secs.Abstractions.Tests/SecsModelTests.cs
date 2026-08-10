using Dreamine.Secs.Abstractions.Hsms;
using Dreamine.Secs.Abstractions.Model;
using Xunit;

namespace Dreamine.Secs.Abstractions.Tests;

public sealed class SecsModelTests
{
    [Fact]
    public void SessionStreamAndFunctionEnforceConfirmedRanges()
    {
        Assert.Equal((ushort)32767, new SecsSessionId(32767).Value);
        Assert.Throws<ArgumentOutOfRangeException>(() => new SecsSessionId(32768));
        Assert.Equal((byte)0, new SecsStream(0).Value);
        Assert.Throws<ArgumentOutOfRangeException>(() => new SecsStream(128));
        Assert.True(new SecsFunction(1).IsPrimary);
        Assert.True(new SecsFunction(2).IsSecondary);
        Assert.False(new SecsFunction(0).IsPrimary);
    }


    [Fact]
    public void MessageRejectsWBitOnSecondaryOrFunctionZero()
    {
        Assert.Throws<ArgumentException>(() => CreateMessage(new SecsFunction(2), true));
        Assert.Throws<ArgumentException>(() => CreateMessage(new SecsFunction(0), true));
        Assert.False(CreateMessage(new SecsFunction(2), false).ReplyExpected);
    }

    [Fact]
    public void ItemsCopyCallerOwnedArrays()
    {
        var source = new byte[] { 1, 2 };
        var item = new SecsBinaryItem(source);
        source[0] = 9;
        Assert.Equal(new byte[] { 1, 2 }, item.Values.ToArray());
    }

    [Fact]
    public void AsciiRejectsNonAsciiCharacters()
    {
        Assert.Throws<ArgumentException>(() => new SecsAsciiItem("한"));
    }

    [Fact]
    public void HsmsDataHeaderMapsSecsFields()
    {
        var message = CreateMessage(new SecsFunction(1), true);
        var header = HsmsHeader.CreateData(message);
        Assert.Equal((ushort)3, header.SessionId);
        Assert.Equal((byte)1, header.Stream);
        Assert.Equal((byte)1, header.Function);
        Assert.True(header.ReplyExpected);
        Assert.Equal((byte)0, header.PType);
        Assert.Equal((byte)HsmsSType.Data, header.SType);
    }

    [Fact]
    public void ActivePassiveModeIsIndependentFromHostEquipmentRole()
    {
        foreach (var mode in Enum.GetValues<Enums.SecsConnectionMode>())
        foreach (var role in Enum.GetValues<Enums.SecsRole>())
        {
            var options = new HsmsSessionOptions { Mode = mode, Role = role };
            options.Validate();
        }
    }

    [Fact]
    public void TimerDefaultsAndRangesMatchTracedRevision()
    {
        var timers = new HsmsTimerOptions();
        Assert.Equal(TimeSpan.FromSeconds(45), timers.T3);
        Assert.Equal(TimeSpan.FromSeconds(10), timers.T5);
        Assert.Equal(TimeSpan.FromSeconds(5), timers.T6);
        Assert.Equal(TimeSpan.FromSeconds(10), timers.T7);
        Assert.Equal(TimeSpan.FromSeconds(5), timers.T8);
        timers.Validate();
        Assert.Throws<ArgumentOutOfRangeException>(() => new HsmsTimerOptions { T3 = TimeSpan.Zero }.Validate());
    }

    private static SecsMessage CreateMessage(SecsFunction function, bool replyExpected) =>
        new(new SecsSessionId(3), new SecsStream(1), function, replyExpected, new SecsSystemBytes(0x01020304));
}
