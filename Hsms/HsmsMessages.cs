using Dreamine.Secs.Abstractions.Model;

namespace Dreamine.Secs.Abstractions.Hsms;

/// <summary>\if KO <para>HSMS의 10바이트 헤더를 타입 안전하게 표현합니다.</para> \endif \if EN <para>Represents the ten-byte HSMS header with typed accessors.</para> \endif</summary>
public readonly record struct HsmsHeader
{
    /// <summary>\if KO 헤더를 만듭니다. \endif \if EN Creates a header. \endif</summary>
    /// <param name="sessionId">\if KO 세션 필드입니다. \endif \if EN Session field. \endif</param><param name="headerByte2">\if KO 세 번째 헤더 바이트입니다. \endif \if EN Header byte two. \endif</param><param name="headerByte3">\if KO 네 번째 헤더 바이트입니다. \endif \if EN Header byte three. \endif</param><param name="pType">\if KO PType입니다. \endif \if EN PType. \endif</param><param name="sType">\if KO SType입니다. \endif \if EN SType. \endif</param><param name="systemBytes">\if KO 시스템 바이트입니다. \endif \if EN System bytes. \endif</param>
    public HsmsHeader(ushort sessionId, byte headerByte2, byte headerByte3, byte pType, byte sType, SecsSystemBytes systemBytes)
    {
        SessionId = sessionId; HeaderByte2 = headerByte2; HeaderByte3 = headerByte3; PType = pType; SType = sType; SystemBytes = systemBytes;
    }

    /// <summary>\if KO 세션 필드입니다. \endif \if EN Gets the session field. \endif</summary>
    public ushort SessionId { get; }
    /// <summary>\if KO 스트림 또는 제어 관련 값입니다. \endif \if EN Gets the stream or control-related value. \endif</summary>
    public byte HeaderByte2 { get; }
    /// <summary>\if KO 함수 또는 상태/사유 값입니다. \endif \if EN Gets the function or status/reason value. \endif</summary>
    public byte HeaderByte3 { get; }
    /// <summary>\if KO 표현 형식입니다. \endif \if EN Gets the presentation type. \endif</summary>
    public byte PType { get; }
    /// <summary>\if KO 세션 형식 원시 값입니다. \endif \if EN Gets the raw session type. \endif</summary>
    public byte SType { get; }
    /// <summary>\if KO 시스템 바이트입니다. \endif \if EN Gets the system bytes. \endif</summary>
    public SecsSystemBytes SystemBytes { get; }
    /// <summary>\if KO 데이터 메시지 여부입니다. \endif \if EN Gets whether this is a data header. \endif</summary>
    public bool IsData => SType == (byte)HsmsSType.Data;
    /// <summary>\if KO 데이터 헤더의 W-bit입니다. \endif \if EN Gets the W-bit of a data header. \endif</summary>
    public bool ReplyExpected => IsData && (HeaderByte2 & 0x80) != 0;
    /// <summary>\if KO 데이터 헤더의 스트림 번호입니다. \endif \if EN Gets the stream number of a data header. \endif</summary>
    public byte Stream => (byte)(HeaderByte2 & 0x7f);
    /// <summary>\if KO 데이터 헤더의 함수 번호입니다. \endif \if EN Gets the function number of a data header. \endif</summary>
    public byte Function => HeaderByte3;

    /// <summary>\if KO SECS 메시지로 데이터 헤더를 만듭니다. \endif \if EN Creates a data header from a SECS message. \endif</summary>
    /// <param name="message">\if KO 메시지입니다. \endif \if EN Message. \endif</param><returns>\if KO 데이터 헤더입니다. \endif \if EN Data header. \endif</returns>
    public static HsmsHeader CreateData(SecsMessage message)
    {
        ArgumentNullException.ThrowIfNull(message);
        var stream = (byte)(message.Stream.Value | (message.ReplyExpected ? 0x80 : 0));
        return new(message.SessionId.Value, stream, message.Function.Value, 0, (byte)HsmsSType.Data, message.SystemBytes);
    }

    /// <summary>\if KO 제어 헤더를 만듭니다. \endif \if EN Creates a control header. \endif</summary>
    /// <param name="sType">\if KO 제어 형식입니다. \endif \if EN Control type. \endif</param><param name="systemBytes">\if KO 시스템 바이트입니다. \endif \if EN System bytes. \endif</param><param name="headerByte2">\if KO 제어 관련 값입니다. \endif \if EN Control-related value. \endif</param><param name="headerByte3">\if KO 상태/사유 값입니다. \endif \if EN Status/reason value. \endif</param><param name="sessionId">\if KO 세션 필드입니다. \endif \if EN Session field. \endif</param><returns>\if KO 제어 헤더입니다. \endif \if EN Control header. \endif</returns>
    public static HsmsHeader CreateControl(HsmsSType sType, SecsSystemBytes systemBytes, byte headerByte2 = 0, byte headerByte3 = 0, ushort sessionId = ushort.MaxValue)
    {
        if (sType == HsmsSType.Data) throw new ArgumentOutOfRangeException(nameof(sType));
        return new(sessionId, headerByte2, headerByte3, 0, (byte)sType, systemBytes);
    }
}

/// <summary>\if KO <para>HSMS 데이터 또는 제어 메시지의 기본 형식입니다.</para> \endif \if EN <para>Base type for an HSMS data or control message.</para> \endif</summary>
public abstract class HsmsMessage
{
    /// <summary>\if KO 헤더로 메시지를 초기화합니다. \endif \if EN Initializes the message with a header. \endif</summary><param name="header">\if KO 헤더입니다. \endif \if EN Header. \endif</param>
    protected HsmsMessage(HsmsHeader header) => Header = header;
    /// <summary>\if KO 10바이트 헤더 모델입니다. \endif \if EN Gets the ten-byte header model. \endif</summary>
    public HsmsHeader Header { get; }
}

/// <summary>\if KO <para>SECS-II 본문을 포함하는 HSMS 데이터 메시지입니다.</para> \endif \if EN <para>Represents an HSMS data message containing a SECS-II body.</para> \endif</summary>
public sealed class HsmsDataMessage : HsmsMessage
{
    /// <summary>\if KO 데이터 메시지를 만듭니다. \endif \if EN Creates a data message. \endif</summary><param name="message">\if KO SECS 메시지입니다. \endif \if EN SECS message. \endif</param>
    public HsmsDataMessage(SecsMessage message) : base(HsmsHeader.CreateData(message))
    {
        SecsMessage = message;
    }
    /// <summary>\if KO SECS 메시지입니다. \endif \if EN Gets the SECS message. \endif</summary>
    public SecsMessage SecsMessage { get; }
}

/// <summary>\if KO <para>본문이 없는 HSMS 제어 메시지입니다.</para> \endif \if EN <para>Represents an HSMS control message without message text.</para> \endif</summary>
public sealed class HsmsControlMessage : HsmsMessage
{
    /// <summary>\if KO 제어 메시지를 만듭니다. \endif \if EN Creates a control message. \endif</summary><param name="header">\if KO 제어 헤더입니다. \endif \if EN Control header. \endif</param>
    public HsmsControlMessage(HsmsHeader header) : base(header)
    {
        if (header.IsData) throw new ArgumentException("A control message cannot use SType 0.", nameof(header));
    }
    /// <summary>\if KO 알려진 SType을 가져옵니다. \endif \if EN Gets the known SType. \endif</summary>
    public HsmsSType SType => (HsmsSType)Header.SType;
}
