using Dreamine.Secs.Abstractions.Enums;

namespace Dreamine.Secs.Abstractions.Model;

/// <summary>
/// \if KO
/// <para>하나의 SECS message session과 현재 TCP 연결 epoch를 식별하는 불변 snapshot입니다.</para>
/// <para>Connection Epoch 0은 아직 성공한 TCP 연결이 없음을 뜻합니다. 성공한 연결의 inbound 문맥은 양수 epoch를 사용해야 합니다.</para>
/// \endif
/// \if EN
/// <para>Represents an immutable snapshot identifying one SECS message session and its current TCP connection epoch.</para>
/// <para>Connection epoch zero means that no TCP connection has succeeded yet. Inbound context from a successful connection must use a positive epoch.</para>
/// \endif
/// </summary>
public sealed record SecsConnectionIdentity
{
    /// <summary>\if KO 검증된 연결 식별 snapshot을 만듭니다. \endif \if EN Creates a validated connection-identity snapshot. \endif</summary>
    /// <param name="providerKey">\if KO 안정적인 공급자 키입니다. \endif \if EN The stable provider key. \endif</param>
    /// <param name="sessionInstanceId">\if KO process 수명 동안 안정적인 non-empty session instance ID입니다. \endif \if EN A stable, non-empty session instance ID for the process lifetime. \endif</param>
    /// <param name="connectionEpoch">\if KO 아직 연결되지 않았으면 0이고, 성공한 TCP 연결마다 증가하는 epoch입니다. \endif \if EN Zero before the first connection, otherwise the epoch increasing for each successful TCP connection. \endif</param>
    /// <param name="sessionId">\if KO 설정된 SECS Session ID입니다. \endif \if EN The configured SECS session ID. \endif</param>
    /// <param name="role">\if KO 설정된 Host 또는 Equipment 역할입니다. \endif \if EN The configured Host or Equipment role. \endif</param>
    /// <param name="mode">\if KO 설정된 Active 또는 Passive 모드입니다. \endif \if EN The configured Active or Passive mode. \endif</param>
    public SecsConnectionIdentity(
        string providerKey,
        Guid sessionInstanceId,
        long connectionEpoch,
        SecsSessionId sessionId,
        SecsRole role,
        SecsConnectionMode mode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(providerKey);
        if (sessionInstanceId == Guid.Empty)
            throw new ArgumentException("A session instance ID cannot be empty.", nameof(sessionInstanceId));
        if (connectionEpoch < 0)
            throw new ArgumentOutOfRangeException(nameof(connectionEpoch));
        if (!Enum.IsDefined(role) || role == SecsRole.Unspecified)
            throw new ArgumentOutOfRangeException(nameof(role));
        if (!Enum.IsDefined(mode) || mode == SecsConnectionMode.Unspecified)
            throw new ArgumentOutOfRangeException(nameof(mode));

        ProviderKey = providerKey;
        SessionInstanceId = sessionInstanceId;
        ConnectionEpoch = connectionEpoch;
        SessionId = sessionId;
        Role = role;
        Mode = mode;
    }

    /// <summary>\if KO 안정적인 공급자 키를 가져옵니다. \endif \if EN Gets the stable provider key. \endif</summary>
    public string ProviderKey { get; }

    /// <summary>\if KO process 수명 동안 안정적인 session instance ID를 가져옵니다. \endif \if EN Gets the session instance ID stable for the process lifetime. \endif</summary>
    public Guid SessionInstanceId { get; }

    /// <summary>\if KO 성공한 TCP 연결 epoch를 가져옵니다. 0은 아직 성공한 연결이 없음을 뜻합니다. \endif \if EN Gets the successful TCP connection epoch; zero means no connection has succeeded yet. \endif</summary>
    public long ConnectionEpoch { get; }

    /// <summary>\if KO 설정된 SECS Session ID를 가져옵니다. \endif \if EN Gets the configured SECS session ID. \endif</summary>
    public SecsSessionId SessionId { get; }

    /// <summary>\if KO 설정된 SECS 역할을 가져옵니다. \endif \if EN Gets the configured SECS role. \endif</summary>
    public SecsRole Role { get; }

    /// <summary>\if KO 설정된 연결 모드를 가져옵니다. \endif \if EN Gets the configured connection mode. \endif</summary>
    public SecsConnectionMode Mode { get; }
}
