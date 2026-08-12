using Dreamine.Communication.Abstractions.Enums;
using Dreamine.Secs.Abstractions.Model;

namespace Dreamine.Secs.Abstractions.Hsms;

/// <summary>
/// \if KO
/// <para>하나의 session에서 관찰된 typed TCP 및 HSMS 상태 전이의 불변 snapshot입니다.</para>
/// <para>두 계층 중 하나만 변경된 경우 변경되지 않은 계층의 previous/current 값은 같을 수 있습니다.</para>
/// \endif
/// \if EN
/// <para>Represents an immutable snapshot of a typed TCP and HSMS state transition observed by one session.</para>
/// <para>When only one layer changes, the previous and current values of the unchanged layer can be equal.</para>
/// \endif
/// </summary>
public sealed class SecsSessionStateChangedEventArgs : EventArgs
{
    /// <summary>\if KO 검증된 상태 전이 snapshot을 만듭니다. \endif \if EN Creates a validated state-transition snapshot. \endif</summary>
    /// <param name="previousConnectionState">\if KO 이전 TCP lifecycle 상태입니다. \endif \if EN The previous TCP lifecycle state. \endif</param>
    /// <param name="currentConnectionState">\if KO 현재 TCP lifecycle 상태입니다. \endif \if EN The current TCP lifecycle state. \endif</param>
    /// <param name="previousHsmsState">\if KO 이전 HSMS protocol 상태입니다. \endif \if EN The previous HSMS protocol state. \endif</param>
    /// <param name="currentHsmsState">\if KO 현재 HSMS protocol 상태입니다. \endif \if EN The current HSMS protocol state. \endif</param>
    /// <param name="connectionIdentity">\if KO 상태를 관찰한 session과 현재 connection epoch identity입니다. \endif \if EN The session and current connection-epoch identity that observed the state. \endif</param>
    public SecsSessionStateChangedEventArgs(
        ConnectionState previousConnectionState,
        ConnectionState currentConnectionState,
        HsmsConnectionState previousHsmsState,
        HsmsConnectionState currentHsmsState,
        SecsConnectionIdentity connectionIdentity)
    {
        if (!Enum.IsDefined(previousConnectionState))
            throw new ArgumentOutOfRangeException(nameof(previousConnectionState));
        if (!Enum.IsDefined(currentConnectionState))
            throw new ArgumentOutOfRangeException(nameof(currentConnectionState));
        if (!Enum.IsDefined(previousHsmsState))
            throw new ArgumentOutOfRangeException(nameof(previousHsmsState));
        if (!Enum.IsDefined(currentHsmsState))
            throw new ArgumentOutOfRangeException(nameof(currentHsmsState));

        PreviousConnectionState = previousConnectionState;
        CurrentConnectionState = currentConnectionState;
        PreviousHsmsState = previousHsmsState;
        CurrentHsmsState = currentHsmsState;
        ConnectionIdentity = connectionIdentity ?? throw new ArgumentNullException(nameof(connectionIdentity));
    }

    /// <summary>\if KO 이전 TCP lifecycle 상태를 가져옵니다. \endif \if EN Gets the previous TCP lifecycle state. \endif</summary>
    public ConnectionState PreviousConnectionState { get; }

    /// <summary>\if KO 현재 TCP lifecycle 상태를 가져옵니다. \endif \if EN Gets the current TCP lifecycle state. \endif</summary>
    public ConnectionState CurrentConnectionState { get; }

    /// <summary>\if KO 이전 HSMS protocol 상태를 가져옵니다. \endif \if EN Gets the previous HSMS protocol state. \endif</summary>
    public HsmsConnectionState PreviousHsmsState { get; }

    /// <summary>\if KO 현재 HSMS protocol 상태를 가져옵니다. \endif \if EN Gets the current HSMS protocol state. \endif</summary>
    public HsmsConnectionState CurrentHsmsState { get; }

    /// <summary>\if KO 이 전이를 관찰한 session과 현재 connection epoch identity를 가져옵니다. \endif \if EN Gets the session and current connection-epoch identity that observed this transition. \endif</summary>
    public SecsConnectionIdentity ConnectionIdentity { get; }
}
