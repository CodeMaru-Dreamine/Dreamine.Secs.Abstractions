namespace Dreamine.Secs.Abstractions.Model;

/// <summary>\if KO <para>불변 SECS-II 데이터 메시지입니다.</para> \endif \if EN <para>Represents an immutable SECS-II data message.</para> \endif</summary>
public sealed class SecsMessage
{
    /// <summary>\if KO <para>메시지를 만듭니다.</para> \endif \if EN <para>Creates a message.</para> \endif</summary>
    /// <param name="sessionId">\if KO 세션 식별자입니다. \endif \if EN The session identifier. \endif</param>
    /// <param name="stream">\if KO 스트림입니다. \endif \if EN The stream. \endif</param>
    /// <param name="function">\if KO 함수입니다. \endif \if EN The function. \endif</param>
    /// <param name="replyExpected">\if KO 응답 기대 여부입니다. \endif \if EN Whether a reply is expected. \endif</param>
    /// <param name="systemBytes">\if KO 시스템 바이트입니다. \endif \if EN The system bytes. \endif</param>
    /// <param name="item">\if KO 선택적 메시지 본문입니다. \endif \if EN The optional message body. \endif</param>
    public SecsMessage(SecsSessionId sessionId, SecsStream stream, SecsFunction function, bool replyExpected, SecsSystemBytes systemBytes, SecsItem? item = null)
    {
        if (replyExpected && !function.IsPrimary)
            throw new ArgumentException("The W-bit is permitted only on a primary message.", nameof(replyExpected));
        SessionId = sessionId;
        Stream = stream;
        Function = function;
        ReplyExpected = replyExpected;
        SystemBytes = systemBytes;
        Item = item;
    }

    /// <summary>\if KO 세션 식별자입니다. \endif \if EN Gets the session identifier. \endif</summary>
    public SecsSessionId SessionId { get; }
    /// <summary>\if KO 스트림입니다. \endif \if EN Gets the stream. \endif</summary>
    public SecsStream Stream { get; }
    /// <summary>\if KO 함수입니다. \endif \if EN Gets the function. \endif</summary>
    public SecsFunction Function { get; }
    /// <summary>\if KO 응답 기대 여부입니다. \endif \if EN Gets whether a reply is expected. \endif</summary>
    public bool ReplyExpected { get; }
    /// <summary>\if KO 시스템 바이트입니다. \endif \if EN Gets the system bytes. \endif</summary>
    public SecsSystemBytes SystemBytes { get; }
    /// <summary>\if KO 선택적 본문 item입니다. \endif \if EN Gets the optional body item. \endif</summary>
    public SecsItem? Item { get; }
}
