using Dreamine.Secs.Abstractions.Hsms;

namespace Dreamine.Secs.Abstractions.Validation;

/// <summary>\if KO <para>프로토콜 검증 오류 종류입니다.</para> \endif \if EN <para>Identifies a protocol validation error.</para> \endif</summary>
public enum SecsValidationCode
{
    /// <summary>\if KO 오류가 없습니다. \endif \if EN No error. \endif</summary>
    None,
    /// <summary>\if KO 데이터가 잘렸습니다. \endif \if EN Data is truncated. \endif</summary>
    Truncated,
    /// <summary>\if KO 길이가 잘못되었습니다. \endif \if EN Length is invalid. \endif</summary>
    InvalidLength,
    /// <summary>\if KO 형식이 지원되지 않습니다. \endif \if EN Format is unsupported. \endif</summary>
    UnsupportedFormat,
    /// <summary>\if KO 값이 범위를 벗어났습니다. \endif \if EN A value is out of range. \endif</summary>
    OutOfRange,
    /// <summary>\if KO 크기 제한을 초과했습니다. \endif \if EN A size limit was exceeded. \endif</summary>
    SizeLimitExceeded,
    /// <summary>\if KO 중첩 깊이 제한을 초과했습니다. \endif \if EN The nesting-depth limit was exceeded. \endif</summary>
    DepthLimitExceeded,
    /// <summary>\if KO 입력 뒤에 소비되지 않은 데이터가 있습니다. \endif \if EN Unconsumed input remains. \endif</summary>
    TrailingData,
    /// <summary>\if KO 메시지 상태 또는 조합이 잘못되었습니다. \endif \if EN A message state or combination is invalid. \endif</summary>
    InvalidMessage,
    /// <summary>\if KO 프로토콜 형식이 지원되지 않습니다. \endif \if EN A protocol type is unsupported. \endif</summary>
    UnsupportedProtocolType
}

/// <summary>\if KO <para>성공 또는 구조화된 프로토콜 오류를 반환합니다.</para> \endif \if EN <para>Returns either success or a structured protocol error.</para> \endif</summary>
public sealed class SecsValidationResult
{
    private SecsValidationResult(bool isValid, SecsValidationCode code, string? message, int? offset)
    {
        IsValid = isValid; Code = code; Message = message; Offset = offset;
    }

    /// <summary>\if KO 성공 결과입니다. \endif \if EN Gets the successful result. \endif</summary>
    public static SecsValidationResult Success { get; } = new(true, SecsValidationCode.None, null, null);
    /// <summary>\if KO 검증 성공 여부입니다. \endif \if EN Gets whether validation succeeded. \endif</summary>
    public bool IsValid { get; }
    /// <summary>\if KO 오류 코드입니다. \endif \if EN Gets the error code. \endif</summary>
    public SecsValidationCode Code { get; }
    /// <summary>\if KO 진단 메시지입니다. \endif \if EN Gets the diagnostic message. \endif</summary>
    public string? Message { get; }
    /// <summary>\if KO 관련 바이트 오프셋입니다. \endif \if EN Gets the related byte offset. \endif</summary>
    public int? Offset { get; }

    /// <summary>\if KO <para>실패 결과를 만듭니다.</para> \endif \if EN <para>Creates a failed result.</para> \endif</summary>
    /// <param name="code">\if KO 오류 코드입니다. \endif \if EN The error code. \endif</param><param name="message">\if KO 메시지입니다. \endif \if EN The message. \endif</param><param name="offset">\if KO 오프셋입니다. \endif \if EN The offset. \endif</param>
    public static SecsValidationResult Failure(SecsValidationCode code, string message, int? offset = null)
    {
        if (code == SecsValidationCode.None) throw new ArgumentOutOfRangeException(nameof(code));
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        return new(false, code, message, offset);
    }
}

/// <summary>\if KO <para>SECS/HSMS 프로토콜 오류의 기본 예외입니다.</para> \endif \if EN <para>Base exception for SECS/HSMS protocol errors.</para> \endif</summary>
public class SecsProtocolException : Exception
{
    /// <summary>\if KO 오류 정보로 예외를 만듭니다. \endif \if EN Creates the exception from validation information. \endif</summary>
    /// <param name="code">\if KO 오류 코드입니다. \endif \if EN Error code. \endif</param><param name="message">\if KO 메시지입니다. \endif \if EN Message. \endif</param><param name="offset">\if KO 오프셋입니다. \endif \if EN Offset. \endif</param>
    public SecsProtocolException(SecsValidationCode code, string message, int? offset = null) : base(message)
    {
        Code = code; Offset = offset;
    }
    /// <summary>\if KO 오류 코드입니다. \endif \if EN Gets the error code. \endif</summary>
    public SecsValidationCode Code { get; }
    /// <summary>\if KO 바이트 오프셋입니다. \endif \if EN Gets the byte offset. \endif</summary>
    public int? Offset { get; }
}

/// <summary>\if KO <para>잘리거나 잘못된 wire data를 나타냅니다.</para> \endif \if EN <para>Indicates truncated or malformed wire data.</para> \endif</summary>
public sealed class SecsDecodeException : SecsProtocolException
{
    /// <summary>\if KO 디코딩 예외를 만듭니다. \endif \if EN Creates a decoding exception. \endif</summary>
    /// <param name="code">\if KO 오류 코드입니다. \endif \if EN Error code. \endif</param><param name="message">\if KO 메시지입니다. \endif \if EN Message. \endif</param><param name="offset">\if KO 오프셋입니다. \endif \if EN Offset. \endif</param>
    public SecsDecodeException(SecsValidationCode code, string message, int? offset = null)
        : this(code, message, offset, null) { }

    /// <summary>\if KO 사용 가능한 HSMS 헤더 문맥을 포함하여 디코딩 예외를 만듭니다. \endif \if EN Creates a decoding exception with available HSMS header context. \endif</summary>
    /// <param name="code">\if KO 오류 코드입니다. \endif \if EN Error code. \endif</param><param name="message">\if KO 메시지입니다. \endif \if EN Message. \endif</param><param name="offset">\if KO 오프셋입니다. \endif \if EN Offset. \endif</param><param name="hsmsHeader">\if KO 완전한 헤더를 읽은 경우의 선택적 문맥입니다. \endif \if EN Optional context when a complete header was read. \endif</param>
    public SecsDecodeException(SecsValidationCode code, string message, int? offset, HsmsHeader? hsmsHeader)
        : base(code, message, offset) => HsmsHeader = hsmsHeader;

    /// <summary>\if KO 오류 전에 완전히 읽은 HSMS 헤더 문맥입니다. \endif \if EN Gets the complete HSMS header context read before the error. \endif</summary>
    public HsmsHeader? HsmsHeader { get; }
}

/// <summary>\if KO <para>트랜잭션 제한 시간 만료를 나타냅니다.</para> \endif \if EN <para>Indicates transaction timeout expiration.</para> \endif</summary>
public sealed class SecsTransactionTimeoutException : TimeoutException
{
    /// <summary>\if KO 시스템 바이트와 제한 시간으로 예외를 만듭니다. \endif \if EN Creates the exception with system bytes and timeout. \endif</summary>
    /// <param name="systemBytes">\if KO 시스템 바이트입니다. \endif \if EN System bytes. \endif</param><param name="timeout">\if KO 제한 시간입니다. \endif \if EN Timeout. \endif</param>
    public SecsTransactionTimeoutException(Model.SecsSystemBytes systemBytes, TimeSpan timeout)
        : base($"SECS transaction 0x{systemBytes.Value:X8} exceeded {timeout}.")
    {
        SystemBytes = systemBytes;
    }
    /// <summary>\if KO 만료된 시스템 바이트입니다. \endif \if EN Gets the expired system bytes. \endif</summary>
    public Model.SecsSystemBytes SystemBytes { get; }
}

/// <summary>\if KO <para>HSMS 타이머 만료를 나타냅니다.</para> \endif \if EN <para>Indicates expiration of an HSMS timer.</para> \endif</summary>
public sealed class HsmsTimerExpiredException : TimeoutException
{
    /// <summary>\if KO 타이머 이름과 기간으로 예외를 만듭니다. \endif \if EN Creates the exception with a timer name and duration. \endif</summary>
    /// <param name="timerName">\if KO 타이머 이름입니다. \endif \if EN Timer name. \endif</param><param name="timeout">\if KO 기간입니다. \endif \if EN Duration. \endif</param>
    public HsmsTimerExpiredException(string timerName, TimeSpan timeout) : base($"HSMS {timerName} expired after {timeout}.")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(timerName);
        TimerName = timerName; Timeout = timeout;
    }
    /// <summary>\if KO 만료된 타이머 이름입니다. \endif \if EN Gets the expired timer name. \endif</summary>
    public string TimerName { get; }
    /// <summary>\if KO 설정된 기간입니다. \endif \if EN Gets the configured duration. \endif</summary>
    public TimeSpan Timeout { get; }
}

/// <summary>\if KO <para>현재 HSMS 상태에서 허용되지 않은 작업을 나타냅니다.</para> \endif \if EN <para>Indicates an operation not permitted in the current HSMS state.</para> \endif</summary>
public sealed class HsmsStateException : InvalidOperationException
{
    /// <summary>\if KO 상태와 작업으로 예외를 만듭니다. \endif \if EN Creates the exception with state and operation. \endif</summary>
    /// <param name="state">\if KO 현재 상태입니다. \endif \if EN Current state. \endif</param><param name="operation">\if KO 작업입니다. \endif \if EN Operation. \endif</param>
    public HsmsStateException(Hsms.HsmsConnectionState state, string operation) : base($"HSMS operation '{operation}' is invalid in state {state}.")
    {
        State = state; Operation = operation;
    }
    /// <summary>\if KO 현재 상태입니다. \endif \if EN Gets the current state. \endif</summary>
    public Hsms.HsmsConnectionState State { get; }
    /// <summary>\if KO 거부된 작업입니다. \endif \if EN Gets the rejected operation. \endif</summary>
    public string Operation { get; }
}
