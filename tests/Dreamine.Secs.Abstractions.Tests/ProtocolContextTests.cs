using Dreamine.Secs.Abstractions.Diagnostics;
using Dreamine.Secs.Abstractions.Hsms;
using Dreamine.Secs.Abstractions.Model;
using Dreamine.Secs.Abstractions.Validation;
using Xunit;

namespace Dreamine.Secs.Abstractions.Tests;

public sealed class ProtocolContextTests
{
    private static readonly HsmsHeader Header = new(
        sessionId: 7,
        headerByte2: 0,
        headerByte3: 0,
        pType: 1,
        sType: 8,
        systemBytes: new SecsSystemBytes(0x10203040));

    [Fact]
    public void ExistingDecodeExceptionConstructorHasNoHsmsHeaderContext()
    {
        var exception = new SecsDecodeException(SecsValidationCode.Truncated, "truncated", 4);

        Assert.Null(exception.HsmsHeader);
    }

    [Fact]
    public void DecodeExceptionAdditiveOverloadRetainsTypedHsmsHeaderContext()
    {
        var exception = new SecsDecodeException(SecsValidationCode.UnsupportedProtocolType, "unsupported", 8, Header);

        Assert.Equal(Header, exception.HsmsHeader);
    }

    [Fact]
    public void ExistingDiagnosticConstructorHasNoHsmsHeaderContext()
    {
        var diagnostic = new SecsDiagnosticEvent(SecsDiagnosticKind.ProtocolError, "protocol error");

        Assert.Null(diagnostic.HsmsHeader);
    }

    [Fact]
    public void DiagnosticAdditiveOverloadRetainsTypedHsmsHeaderContext()
    {
        var diagnostic = new SecsDiagnosticEvent(
            SecsDiagnosticKind.ProtocolError,
            "protocol error",
            HsmsConnectionState.Selected,
            frameLength: 14,
            hsmsHeader: Header);

        Assert.Equal(Header, diagnostic.HsmsHeader);
    }
}
