using Dreamine.Secs.Abstractions.Hsms;

namespace Dreamine.Secs.Abstractions.Diagnostics;

/// <summary>\if KO <para>SECS/HSMS 진단 이벤트 종류입니다.</para> \endif \if EN <para>Identifies a SECS/HSMS diagnostic event.</para> \endif</summary>
public enum SecsDiagnosticKind
{
    /// <summary>\if KO 연결 시도입니다. \endif \if EN Connection attempt. \endif</summary>
    ConnectionAttempt,
    /// <summary>\if KO 연결 성공입니다. \endif \if EN Connection established. \endif</summary>
    ConnectionEstablished,
    /// <summary>\if KO 연결 종료입니다. \endif \if EN Connection closed. \endif</summary>
    ConnectionClosed,
    /// <summary>\if KO 상태 변경입니다. \endif \if EN State changed. \endif</summary>
    StateChanged,
    /// <summary>\if KO 프레임 송신입니다. \endif \if EN Frame sent. \endif</summary>
    FrameSent,
    /// <summary>\if KO 프레임 수신입니다. \endif \if EN Frame received. \endif</summary>
    FrameReceived,
    /// <summary>\if KO 주 메시지 송신입니다. \endif \if EN Primary sent. \endif</summary>
    PrimarySent,
    /// <summary>\if KO 부 메시지 수신입니다. \endif \if EN Secondary received. \endif</summary>
    SecondaryReceived,
    /// <summary>\if KO 제한 시간 만료입니다. \endif \if EN Timeout. \endif</summary>
    Timeout,
    /// <summary>\if KO Reject입니다. \endif \if EN Reject. \endif</summary>
    Reject,
    /// <summary>\if KO 프로토콜 오류입니다. \endif \if EN Protocol error. \endif</summary>
    ProtocolError,
    /// <summary>\if KO 상위 계층 callback 오류입니다. \endif \if EN An upper-layer callback error. \endif</summary>
    ApplicationError
}

/// <summary>\if KO <para>크기가 제한된 진단 레코드입니다.</para> \endif \if EN <para>Represents a size-bounded diagnostic record.</para> \endif</summary>
public sealed class SecsDiagnosticEvent
{
    /// <summary>\if KO 진단 이벤트를 만듭니다. \endif \if EN Creates a diagnostic event. \endif</summary>
    /// <param name="kind">\if KO 종류입니다. \endif \if EN Kind. \endif</param><param name="message">\if KO 요약입니다. \endif \if EN Summary. \endif</param><param name="state">\if KO 선택적 상태입니다. \endif \if EN Optional state. \endif</param><param name="frameLength">\if KO 선택적 프레임 길이입니다. \endif \if EN Optional frame length. \endif</param>
    public SecsDiagnosticEvent(SecsDiagnosticKind kind, string message, HsmsConnectionState? state = null, int? frameLength = null)
        : this(kind, message, state, frameLength, null) { }

    /// <summary>\if KO 사용 가능한 HSMS 헤더 문맥을 포함하여 진단 이벤트를 만듭니다. \endif \if EN Creates a diagnostic event with available HSMS header context. \endif</summary>
    /// <param name="kind">\if KO 종류입니다. \endif \if EN Kind. \endif</param><param name="message">\if KO 요약입니다. \endif \if EN Summary. \endif</param><param name="state">\if KO 선택적 상태입니다. \endif \if EN Optional state. \endif</param><param name="frameLength">\if KO 선택적 프레임 길이입니다. \endif \if EN Optional frame length. \endif</param><param name="hsmsHeader">\if KO 완전한 헤더를 읽은 경우의 선택적 문맥입니다. \endif \if EN Optional context when a complete header was read. \endif</param>
    public SecsDiagnosticEvent(
        SecsDiagnosticKind kind,
        string message,
        HsmsConnectionState? state,
        int? frameLength,
        HsmsHeader? hsmsHeader)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Kind = kind; Message = message; State = state; FrameLength = frameLength; HsmsHeader = hsmsHeader;
    }
    /// <summary>\if KO 종류입니다. \endif \if EN Gets the kind. \endif</summary>
    public SecsDiagnosticKind Kind { get; }
    /// <summary>\if KO payload 전체를 포함하지 않는 요약입니다. \endif \if EN Gets a summary that does not contain an unbounded payload. \endif</summary>
    public string Message { get; }
    /// <summary>\if KO 관련 상태입니다. \endif \if EN Gets the related state. \endif</summary>
    public HsmsConnectionState? State { get; }
    /// <summary>\if KO 관련 프레임 길이입니다. \endif \if EN Gets the related frame length. \endif</summary>
    public int? FrameLength { get; }
    /// <summary>\if KO 오류 전에 완전히 읽은 HSMS 헤더 문맥입니다. \endif \if EN Gets the complete HSMS header context read before the error. \endif</summary>
    public HsmsHeader? HsmsHeader { get; }
}

/// <summary>\if KO <para>특정 로깅 구현과 분리된 진단 수신 계약입니다.</para> \endif \if EN <para>Defines a diagnostic sink decoupled from a logging implementation.</para> \endif</summary>
public interface ISecsDiagnosticSink
{
    /// <summary>\if KO 진단 이벤트를 게시합니다. \endif \if EN Publishes a diagnostic event. \endif</summary>
    /// <param name="diagnosticEvent">\if KO 이벤트입니다. \endif \if EN Event. \endif</param>
    void Emit(SecsDiagnosticEvent diagnosticEvent);
}
