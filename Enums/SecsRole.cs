namespace Dreamine.Secs.Abstractions.Enums;

/// <summary>
/// \if KO
/// <para>SECS 연결에서 애플리케이션이 담당하는 역할을 나타냅니다.</para>
/// \endif
/// \if EN
/// <para>Identifies the application role in a SECS connection.</para>
/// \endif
/// </summary>
public enum SecsRole
{
    /// <summary>
    /// \if KO
    /// <para>역할이 아직 명시되지 않았습니다.</para>
    /// \endif
    /// \if EN
    /// <para>The role has not been specified.</para>
    /// \endif
    /// </summary>
    Unspecified,

    /// <summary>
    /// \if KO
    /// <para>호스트 역할입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The host role.</para>
    /// \endif
    /// </summary>
    Host,

    /// <summary>
    /// \if KO
    /// <para>장비 역할입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The equipment role.</para>
    /// \endif
    /// </summary>
    Equipment
}
