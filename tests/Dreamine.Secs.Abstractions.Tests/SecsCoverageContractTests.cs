using Dreamine.Secs.Abstractions.Hsms;
using Dreamine.Secs.Abstractions.Model;
using Dreamine.Secs.Abstractions.Validation;
using Xunit;

namespace Dreamine.Secs.Abstractions.Tests;

public sealed class SecsCoverageContractTests
{
    public static TheoryData<SecsItem, SecsItemFormat, int, int> AtomicItems => new()
    {
        { new SecsBinaryItem(1, 2), SecsItemFormat.Binary, 2, 2 },
        { new SecsBooleanItem(true, false), SecsItemFormat.Boolean, 2, 2 },
        { new SecsAsciiItem("AB"), SecsItemFormat.Ascii, 2, 2 },
        { new SecsJis8Item(1, 2), SecsItemFormat.Jis8, 2, 2 },
        { new SecsInt8Item(1, 2), SecsItemFormat.Int8, 2, 2 },
        { new SecsInt16Item(1, 2), SecsItemFormat.Int16, 2, 4 },
        { new SecsInt32Item(1, 2), SecsItemFormat.Int32, 2, 8 },
        { new SecsInt64Item(1, 2), SecsItemFormat.Int64, 2, 16 },
        { new SecsUInt8Item(1, 2), SecsItemFormat.UInt8, 2, 2 },
        { new SecsUInt16Item(1, 2), SecsItemFormat.UInt16, 2, 4 },
        { new SecsUInt32Item(1, 2), SecsItemFormat.UInt32, 2, 8 },
        { new SecsUInt64Item(1, 2), SecsItemFormat.UInt64, 2, 16 },
        { new SecsFloat32Item(1, 2), SecsItemFormat.Float32, 2, 8 },
        { new SecsFloat64Item(1, 2), SecsItemFormat.Float64, 2, 16 }
    };

    [Theory]
    [MemberData(nameof(AtomicItems))]
    public void AtomicItemsExposeFormatCountAndEncodedBodyLength(
        SecsItem item,
        SecsItemFormat expectedFormat,
        int expectedCount,
        int expectedBodyLength)
    {
        Assert.Equal(expectedFormat, item.Format);
        Assert.Equal(expectedCount, item.Count);
        Assert.Equal(expectedBodyLength, item.BodyLength);
    }

    [Fact]
    public void ListItemCopiesChildrenAndComputesNestedEncodedLength()
    {
        SecsItem[] children = [new SecsUInt16Item(1), new SecsAsciiItem("A")];
        var list = new SecsListItem(children);
        children[0] = new SecsUInt8Item(9);

        Assert.Equal(SecsItemFormat.List, list.Format);
        Assert.Equal(2, list.Count);
        Assert.Equal(7, list.BodyLength);
        Assert.IsType<SecsUInt16Item>(list.Items[0]);
        Assert.Throws<ArgumentException>(() => new SecsListItem([null!]));
    }

    [Fact]
    public void ValidationResultsAndProtocolExceptionsRetainStructuredContext()
    {
        Assert.True(SecsValidationResult.Success.IsValid);
        Assert.Equal(SecsValidationCode.None, SecsValidationResult.Success.Code);
        Assert.Null(SecsValidationResult.Success.Message);
        Assert.Null(SecsValidationResult.Success.Offset);

        var failure = SecsValidationResult.Failure(SecsValidationCode.Truncated, "short", 4);
        Assert.False(failure.IsValid);
        Assert.Equal(SecsValidationCode.Truncated, failure.Code);
        Assert.Equal("short", failure.Message);
        Assert.Equal(4, failure.Offset);
        Assert.Throws<ArgumentOutOfRangeException>(() => SecsValidationResult.Failure(SecsValidationCode.None, "none"));

        var header = HsmsHeader.CreateControl(HsmsSType.LinktestRequest, new SecsSystemBytes(7));
        var decode = new SecsDecodeException(SecsValidationCode.InvalidLength, "bad", 2, header);
        Assert.Equal(SecsValidationCode.InvalidLength, decode.Code);
        Assert.Equal(2, decode.Offset);
        Assert.Equal(header, decode.HsmsHeader);

        var transaction = new SecsTransactionTimeoutException(new SecsSystemBytes(8), TimeSpan.FromSeconds(1));
        Assert.Equal((uint)8, transaction.SystemBytes.Value);
        var timer = new HsmsTimerExpiredException("T8", TimeSpan.FromSeconds(2));
        Assert.Equal("T8", timer.TimerName);
        Assert.Equal(TimeSpan.FromSeconds(2), timer.Timeout);
        var state = new HsmsStateException(HsmsConnectionState.Selected, "send");
        Assert.Equal(HsmsConnectionState.Selected, state.State);
        Assert.Equal("send", state.Operation);
    }
}
