using Dreamine.Secs.Abstractions.Enums;

namespace Dreamine.Secs.Abstractions.Hsms;

/// <summary>\if KO <para>E37-0413 범위와 기본값을 적용한 HSMS 타이머 설정입니다.</para> \endif \if EN <para>Configures HSMS timers with E37-0413 ranges and defaults.</para> \endif</summary>
public sealed class HsmsTimerOptions
{
    /// <summary>\if KO T3 응답 제한 시간입니다. \endif \if EN Gets the T3 reply timeout. \endif</summary>
    public TimeSpan T3 { get; init; } = TimeSpan.FromSeconds(45);
    /// <summary>\if KO T5 재연결 분리 시간입니다. \endif \if EN Gets the T5 reconnect-separation time. \endif</summary>
    public TimeSpan T5 { get; init; } = TimeSpan.FromSeconds(10);
    /// <summary>\if KO T6 제어 트랜잭션 제한 시간입니다. \endif \if EN Gets the T6 control-transaction timeout. \endif</summary>
    public TimeSpan T6 { get; init; } = TimeSpan.FromSeconds(5);
    /// <summary>\if KO T7 미선택 상태 제한 시간입니다. \endif \if EN Gets the T7 not-selected timeout. \endif</summary>
    public TimeSpan T7 { get; init; } = TimeSpan.FromSeconds(10);
    /// <summary>\if KO T8 바이트 간 제한 시간입니다. \endif \if EN Gets the T8 inter-character timeout. \endif</summary>
    public TimeSpan T8 { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>\if KO 설정값을 표준 범위로 검증합니다. \endif \if EN Validates values against the standard ranges. \endif</summary>
    public void Validate()
    {
        ValidateRange(T3, 1, 120, nameof(T3));
        ValidateRange(T5, 1, 240, nameof(T5));
        ValidateRange(T6, 1, 240, nameof(T6));
        ValidateRange(T7, 1, 240, nameof(T7));
        ValidateRange(T8, 1, 120, nameof(T8));
    }

    private static void ValidateRange(TimeSpan value, int minimumSeconds, int maximumSeconds, string name)
    {
        if (value < TimeSpan.FromSeconds(minimumSeconds) || value > TimeSpan.FromSeconds(maximumSeconds))
            throw new ArgumentOutOfRangeException(name, value, $"{name} must be between {minimumSeconds} and {maximumSeconds} seconds.");
        if (value.Ticks % TimeSpan.TicksPerSecond != 0)
            throw new ArgumentOutOfRangeException(name, value, $"{name} must use one-second resolution.");
    }
}

/// <summary>\if KO <para>단일 HSMS 연결 설정입니다.</para> \endif \if EN <para>Configures one HSMS connection.</para> \endif</summary>
public sealed class HsmsSessionOptions
{
    /// <summary>\if KO 원격 또는 바인딩 호스트입니다. \endif \if EN Gets the remote or bind host. \endif</summary>
    public string Host { get; init; } = "127.0.0.1";
    /// <summary>\if KO TCP 포트입니다. \endif \if EN Gets the TCP port. \endif</summary>
    public int Port { get; init; } = 5000;
    /// <summary>\if KO 능동/수동 TCP 방향입니다. \endif \if EN Gets the active/passive TCP direction. \endif</summary>
    public SecsConnectionMode Mode { get; init; }
    /// <summary>\if KO Host/Equipment SECS 역할입니다. \endif \if EN Gets the Host/Equipment SECS role. \endif</summary>
    public SecsRole Role { get; init; }
    /// <summary>\if KO 데이터 메시지의 세션 식별자입니다. \endif \if EN Gets the data-message session identifier. \endif</summary>
    public Model.SecsSessionId SessionId { get; init; }
    /// <summary>\if KO HSMS 타이머 설정입니다. \endif \if EN Gets the HSMS timer settings. \endif</summary>
    public HsmsTimerOptions Timers { get; init; } = new();
    /// <summary>\if KO 최대 프레임 길이(헤더와 text 포함)입니다. \endif \if EN Gets the maximum frame length including header and text. \endif</summary>
    public int MaximumFrameLength { get; init; } = 16 * 1024 * 1024;
    /// <summary>\if KO 연결 종료 후 능동 재연결 여부입니다. \endif \if EN Gets whether an active endpoint reconnects after disconnection. \endif</summary>
    public bool AutoReconnect { get; init; }

    /// <summary>\if KO 설정을 검증합니다. \endif \if EN Validates the settings. \endif</summary>
    public void Validate()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(Host);
        if (Port is <= 0 or > 65535) throw new ArgumentOutOfRangeException(nameof(Port));
        if (!Enum.IsDefined(Mode)) throw new ArgumentOutOfRangeException(nameof(Mode));
        if (!Enum.IsDefined(Role)) throw new ArgumentOutOfRangeException(nameof(Role));
        if (MaximumFrameLength < 10 || MaximumFrameLength > int.MaxValue - 4) throw new ArgumentOutOfRangeException(nameof(MaximumFrameLength));
        if (AutoReconnect && Mode != SecsConnectionMode.Active) throw new ArgumentException("Automatic T5 reconnect is available only in Active mode.", nameof(AutoReconnect));
        ArgumentNullException.ThrowIfNull(Timers);
        Timers.Validate();
    }
}
