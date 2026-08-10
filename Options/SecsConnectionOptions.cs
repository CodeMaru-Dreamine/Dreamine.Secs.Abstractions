using Dreamine.Secs.Abstractions.Enums;
using Dreamine.Secs.Abstractions.Providers;

namespace Dreamine.Secs.Abstractions.Options;

/// <summary>
/// \if KO
/// <para>공급자 선택과 역할 및 연결 모드에 필요한 최소 SECS 연결 설정입니다.</para>
/// \endif
/// \if EN
/// <para>Defines the minimal SECS connection settings used for provider selection, role, and connection mode.</para>
/// \endif
/// </summary>
public sealed class SecsConnectionOptions
{
    /// <summary>
    /// \if KO
    /// <para>DI 또는 Registry에서 조회할 안정적인 공급자 키를 가져오거나 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or initializes the stable provider key resolved through dependency injection or a registry.</para>
    /// \endif
    /// </summary>
    public string ProviderKey { get; init; } = SecsProviderKeys.Dreamine;

    /// <summary>
    /// \if KO
    /// <para>애플리케이션의 SECS 역할을 가져오거나 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or initializes the application's SECS role.</para>
    /// \endif
    /// </summary>
    public SecsRole Role { get; init; }

    /// <summary>
    /// \if KO
    /// <para>연결의 능동 또는 수동 모드를 가져오거나 초기화합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets or initializes the active or passive connection mode.</para>
    /// \endif
    /// </summary>
    public SecsConnectionMode Mode { get; init; }
}
