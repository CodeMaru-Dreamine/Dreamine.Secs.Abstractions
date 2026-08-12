namespace Dreamine.Secs.Abstractions.Hsms;

/// <summary>\if KO 완전한 HSMS wire frame의 방향을 나타냅니다. \endif \if EN Identifies the direction of a complete HSMS wire frame. \endif</summary>
public enum HsmsWireDirection
{
    /// <summary>\if KO 원격 endpoint에서 읽은 frame입니다. \endif \if EN A frame read from the remote endpoint. \endif</summary>
    Inbound,
    /// <summary>\if KO 원격 endpoint로 기록한 frame입니다. \endif \if EN A frame written to the remote endpoint. \endif</summary>
    Outbound
}

/// <summary>\if KO wire snapshot에서 보존할 byte 범위를 지정합니다. \endif \if EN Specifies how many wire bytes an observation retains. \endif</summary>
public enum HsmsWireCaptureMode
{
    /// <summary>\if KO frame metadata만 보존하고 wire byte는 복사하지 않습니다. \endif \if EN Retains frame metadata without copying wire bytes. \endif</summary>
    Excluded,
    /// <summary>\if KO 4-byte 길이 prefix와 10-byte HSMS header만 보존합니다. \endif \if EN Retains only the four-byte length prefix and ten-byte HSMS header. \endif</summary>
    HeaderOnly,
    /// <summary>\if KO 설정된 byte 상한까지 완전한 frame 앞부분을 보존합니다. \endif \if EN Retains the complete-frame prefix up to the configured byte limit. \endif</summary>
    FullFrame
}

/// <summary>\if KO 특정 방향과 S/F에 적용할 wire capture 규칙입니다. \endif \if EN Defines a wire-capture rule for a direction and S/F. \endif</summary>
public sealed class HsmsWireCaptureRule
{
    /// <summary>\if KO capture 규칙을 만듭니다. \endif \if EN Creates a capture rule. \endif</summary>
    /// <param name="stream">\if KO 데이터 메시지 Stream입니다. \endif \if EN Data-message stream. \endif</param>
    /// <param name="function">\if KO 데이터 메시지 Function입니다. \endif \if EN Data-message function. \endif</param>
    /// <param name="direction">\if KO 적용할 방향이며 null이면 양방향입니다. \endif \if EN Direction to match, or null for both directions. \endif</param>
    /// <param name="mode">\if KO capture 모드입니다. \endif \if EN Capture mode. \endif</param>
    /// <param name="maximumCapturedBytes">\if KO FullFrame에서 보존할 최대 byte입니다. \endif \if EN Maximum retained bytes for FullFrame. \endif</param>
    public HsmsWireCaptureRule(
        byte stream,
        byte function,
        HsmsWireDirection? direction,
        HsmsWireCaptureMode mode,
        int maximumCapturedBytes = 0)
    {
        Stream = stream;
        Function = function;
        Direction = direction;
        Mode = mode;
        MaximumCapturedBytes = maximumCapturedBytes;
    }

    /// <summary>\if KO Stream을 가져옵니다. \endif \if EN Gets the stream. \endif</summary>
    public byte Stream { get; }
    /// <summary>\if KO Function을 가져옵니다. \endif \if EN Gets the function. \endif</summary>
    public byte Function { get; }
    /// <summary>\if KO 적용 방향 또는 양방향을 뜻하는 null을 가져옵니다. \endif \if EN Gets the direction, or null for both directions. \endif</summary>
    public HsmsWireDirection? Direction { get; }
    /// <summary>\if KO capture 모드를 가져옵니다. \endif \if EN Gets the capture mode. \endif</summary>
    public HsmsWireCaptureMode Mode { get; }
    /// <summary>\if KO FullFrame의 byte 상한을 가져옵니다. \endif \if EN Gets the FullFrame byte limit. \endif</summary>
    public int MaximumCapturedBytes { get; }

    internal void Validate(int globalMaximumCapturedBytes)
    {
        if (Stream is 0 or > 127) throw new ArgumentOutOfRangeException(nameof(Stream));
        if (Direction is { } direction && !Enum.IsDefined(direction))
            throw new ArgumentOutOfRangeException(nameof(Direction));
        if (!Enum.IsDefined(Mode)) throw new ArgumentOutOfRangeException(nameof(Mode));
        if (Mode == HsmsWireCaptureMode.FullFrame)
        {
            if (MaximumCapturedBytes < 1 || MaximumCapturedBytes > globalMaximumCapturedBytes)
                throw new ArgumentOutOfRangeException(nameof(MaximumCapturedBytes));
        }
        else if (MaximumCapturedBytes != 0)
        {
            throw new ArgumentException("Only FullFrame rules can set a byte limit.", nameof(MaximumCapturedBytes));
        }
    }
}

/// <summary>
/// \if KO
/// <para>완전한 HSMS wire frame의 opt-in capture를 설정합니다.</para>
/// <para>Capture된 frame에는 민감한 application payload가 포함될 수 있으므로 그에 맞게 보호해야 합니다.</para>
/// \endif
/// \if EN
/// Configures the opt-in capture of complete HSMS wire frames.
/// Captured frames can contain sensitive application payload and must be protected accordingly.
/// \endif
/// </summary>
public sealed class HsmsWireObservationOptions
{
    /// <summary>\if KO bounded observation queue 전체에서 보존할 수 있는 최대 payload byte입니다. \endif \if EN The maximum retained payload budget across the bounded observation queue. \endif</summary>
    public const long MaximumRetainedPayloadBytes = 64L * 1024 * 1024;

    /// <summary>\if KO bounded observation queue capacity를 가져옵니다. \endif \if EN Gets the bounded observation queue capacity. \endif</summary>
    public int QueueCapacity { get; init; } = 256;

    /// <summary>\if KO 각 완전한 frame에서 보존할 최대 byte 수를 가져옵니다. \endif \if EN Gets the maximum number of bytes retained from each complete frame. \endif</summary>
    public int MaximumCapturedBytes { get; init; } = 64 * 1024;

    /// <summary>\if KO 일치하는 S/F 규칙이 없을 때 적용할 capture 모드입니다. 기존 호환성을 위해 기본값은 FullFrame입니다. \endif \if EN Gets the capture mode used when no S/F rule matches. The default remains FullFrame for compatibility. \endif</summary>
    public HsmsWireCaptureMode DefaultCaptureMode { get; init; } = HsmsWireCaptureMode.FullFrame;

    /// <summary>\if KO payload 복사 전에 평가할 S/F capture 규칙을 가져옵니다. 방향별 규칙은 양방향 규칙보다 우선합니다. \endif \if EN Gets S/F capture rules evaluated before payload copying. Direction-specific rules take precedence over bidirectional rules. \endif</summary>
    public IReadOnlyList<HsmsWireCaptureRule> CaptureRules { get; init; } = Array.Empty<HsmsWireCaptureRule>();

    /// <summary>\if KO queue와 retained-payload 제한을 검증합니다. \endif \if EN Validates the queue and retained-payload limits. \endif</summary>
    public void Validate()
    {
        if (QueueCapacity is < 1 or > 65_536)
            throw new ArgumentOutOfRangeException(nameof(QueueCapacity));
        if (MaximumCapturedBytes < 1)
            throw new ArgumentOutOfRangeException(nameof(MaximumCapturedBytes));
        if ((long)QueueCapacity * MaximumCapturedBytes > MaximumRetainedPayloadBytes)
            throw new ArgumentOutOfRangeException(nameof(MaximumCapturedBytes), "The bounded wire-observation queue can retain at most 64 MiB.");
        if (!Enum.IsDefined(DefaultCaptureMode)) throw new ArgumentOutOfRangeException(nameof(DefaultCaptureMode));
        ArgumentNullException.ThrowIfNull(CaptureRules);
        foreach (var rule in CaptureRules)
        {
            ArgumentNullException.ThrowIfNull(rule);
            rule.Validate(MaximumCapturedBytes);
        }
        var duplicate = CaptureRules.GroupBy(rule => (rule.Stream, rule.Function, rule.Direction))
            .FirstOrDefault(group => group.Count() > 1);
        if (duplicate is not null)
            throw new ArgumentException($"Duplicate wire-capture rule for S{duplicate.Key.Stream}F{duplicate.Key.Function}.", nameof(CaptureRules));
    }
}

/// <summary>
/// \if KO
/// <para>4-byte 길이 prefix를 포함한 하나의 완전한 HSMS frame의 불변 snapshot을 나타냅니다.</para>
/// <para>부분 frame은 보고하지 않으며 capture된 byte에는 민감한 application payload가 포함될 수 있습니다.</para>
/// \endif
/// \if EN
/// Describes an immutable snapshot of one complete HSMS frame, including its four-byte length prefix.
/// Partial frames are not reported. Captured bytes may contain sensitive application payload.
/// \endif
/// </summary>
public sealed class HsmsWireObservation
{
    /// <summary>\if KO 완전한 frame observation을 만듭니다. \endif \if EN Creates a complete-frame observation. \endif</summary>
    public HsmsWireObservation(
        long sequenceNumber,
        long connectionEpoch,
        DateTimeOffset observedAtUtc,
        HsmsWireDirection direction,
        int actualByteCount,
        int declaredFrameLength,
        ReadOnlyMemory<byte> capturedBytes)
        : this(sequenceNumber, connectionEpoch, observedAtUtc, direction, actualByteCount,
            declaredFrameLength, capturedBytes, null)
    {
    }

    /// <summary>\if KO typed HSMS header metadata를 포함한 완전한 frame observation을 만듭니다. \endif \if EN Creates a complete-frame observation with typed HSMS header metadata. \endif</summary>
    public HsmsWireObservation(
        long sequenceNumber,
        long connectionEpoch,
        DateTimeOffset observedAtUtc,
        HsmsWireDirection direction,
        int actualByteCount,
        int declaredFrameLength,
        ReadOnlyMemory<byte> capturedBytes,
        HsmsHeader? header)
    {
        if (sequenceNumber <= 0) throw new ArgumentOutOfRangeException(nameof(sequenceNumber));
        if (connectionEpoch <= 0) throw new ArgumentOutOfRangeException(nameof(connectionEpoch));
        if (observedAtUtc.Offset != TimeSpan.Zero) throw new ArgumentException("Observation timestamps must be UTC.", nameof(observedAtUtc));
        if (!Enum.IsDefined(direction)) throw new ArgumentOutOfRangeException(nameof(direction));
        if (declaredFrameLength < 0) throw new ArgumentOutOfRangeException(nameof(declaredFrameLength));
        if (actualByteCount != checked(declaredFrameLength + 4)) throw new ArgumentOutOfRangeException(nameof(actualByteCount));
        if (capturedBytes.Length > actualByteCount) throw new ArgumentOutOfRangeException(nameof(capturedBytes));

        SequenceNumber = sequenceNumber;
        ConnectionEpoch = connectionEpoch;
        ObservedAtUtc = observedAtUtc;
        Direction = direction;
        ActualByteCount = actualByteCount;
        DeclaredFrameLength = declaredFrameLength;
        CapturedBytes = capturedBytes.IsEmpty ? ReadOnlyMemory<byte>.Empty : capturedBytes.ToArray();
        Header = header;
    }

    /// <summary>\if KO session 전체에서 단조 증가하는 observation 시도 번호입니다. 번호의 공백은 drop된 observation을 뜻합니다. \endif \if EN Gets the session-wide monotonic observation attempt number. Gaps represent dropped observations. \endif</summary>
    public long SequenceNumber { get; }
    /// <summary>\if KO 성공한 TCP 연결마다 단조 증가하는 epoch를 가져옵니다. \endif \if EN Gets the monotonically increasing successful TCP connection epoch. \endif</summary>
    public long ConnectionEpoch { get; }
    /// <summary>\if KO UTC observation 시각을 가져옵니다. \endif \if EN Gets the UTC observation timestamp. \endif</summary>
    public DateTimeOffset ObservedAtUtc { get; }
    /// <summary>\if KO wire 방향을 가져옵니다. \endif \if EN Gets the wire direction. \endif</summary>
    public HsmsWireDirection Direction { get; }
    /// <summary>\if KO 4-byte prefix를 포함한 완전한 frame byte 수를 가져옵니다. \endif \if EN Gets the complete frame byte count, including the four-byte prefix. \endif</summary>
    public int ActualByteCount { get; }
    /// <summary>\if KO prefix 자체를 제외하고 prefix가 선언한 길이를 가져옵니다. \endif \if EN Gets the length declared by the prefix, excluding the prefix itself. \endif</summary>
    public int DeclaredFrameLength { get; }
    /// <summary>\if KO 완전한 frame에서 불변으로 보존한 앞부분을 가져옵니다. \endif \if EN Gets an immutable retained prefix of the complete frame. \endif</summary>
    public ReadOnlyMemory<byte> CapturedBytes { get; }
    /// <summary>\if KO payload capture 여부와 무관하게 읽은 typed HSMS header를 가져옵니다. 완전한 header를 읽지 못했으면 null입니다. \endif \if EN Gets the typed HSMS header independently of payload capture, or null when a complete header was unavailable. \endif</summary>
    public HsmsHeader? Header { get; }
    /// <summary>\if KO 설정된 capture 제한 때문에 보존 snapshot이 잘렸는지 가져옵니다. \endif \if EN Gets whether the configured capture limit truncated the retained snapshot. \endif</summary>
    public bool IsCaptureTruncated => CapturedBytes.Length < ActualByteCount;
}

/// <summary>
/// \if KO
/// <para>opt-in HSMS wire observation을 하나의 pull consumer가 읽는 stream을 노출합니다.</para>
/// <para>protocol은 consumer를 호출하거나 기다리지 않습니다.</para>
/// \endif
/// \if EN
/// Exposes a single-pull-consumer stream of opt-in HSMS wire observations.
/// The protocol never waits for or invokes a consumer.
/// \endif
/// </summary>
public interface IHsmsWireObservationSource
{
    /// <summary>\if KO 연결 생성 시 wire observation이 활성화됐는지 가져옵니다. \endif \if EN Gets whether wire observation was enabled when the connection was created. \endif</summary>
    bool IsWireObservationEnabled { get; }
    /// <summary>\if KO bounded queue가 가득 차 drop된 observation 수를 가져옵니다. \endif \if EN Gets the number of observations dropped because the bounded queue was full. \endif</summary>
    long DroppedWireObservationCount { get; }
    /// <summary>\if KO observation을 읽습니다. 동시에 하나의 활성 enumerator만 지원합니다. \endif \if EN Reads observations. Only one active enumerator is supported at a time. \endif</summary>
    IAsyncEnumerable<HsmsWireObservation> ReadWireObservationsAsync(CancellationToken cancellationToken = default);
}
