namespace Dreamine.Secs.Abstractions.Enums;

/// <summary>
/// \if KO
/// <para>SECS 연결 설정의 능동 또는 수동 모드를 나타냅니다.</para>
/// \endif
/// \if EN
/// <para>Identifies the active or passive mode of a SECS connection configuration.</para>
/// \endif
/// </summary>
public enum SecsConnectionMode
{
    /// <summary>
    /// \if KO
    /// <para>연결 모드가 아직 명시되지 않았습니다.</para>
    /// \endif
    /// \if EN
    /// <para>The connection mode has not been specified.</para>
    /// \endif
    /// </summary>
    Unspecified,

    /// <summary>
    /// \if KO
    /// <para>연결을 시작하는 능동 모드입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The active mode that initiates a connection.</para>
    /// \endif
    /// </summary>
    Active,

    /// <summary>
    /// \if KO
    /// <para>연결을 기다리는 수동 모드입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The passive mode that waits for a connection.</para>
    /// \endif
    /// </summary>
    Passive
}
