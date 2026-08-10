using Dreamine.Communication.Abstractions.Interfaces;

namespace Dreamine.Secs.Abstractions.Interfaces;

/// <summary>
/// \if KO
/// <para>공급자와 무관한 SECS 연결 생명주기 계약입니다.</para>
/// \endif
/// \if EN
/// <para>Defines a provider-independent SECS connection lifecycle contract.</para>
/// \endif
/// </summary>
public interface ISecsConnection : IConnectionLifecycle, IAsyncDisposable
{
    /// <summary>
    /// \if KO
    /// <para>이 연결을 생성한 공급자의 안정적인 키를 가져옵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets the stable key of the provider that created this connection.</para>
    /// \endif
    /// </summary>
    string ProviderKey { get; }
}
