using Dreamine.Secs.Abstractions.Options;

namespace Dreamine.Secs.Abstractions.Interfaces;

/// <summary>
/// \if KO
/// <para>벤더 SDK 타입을 노출하지 않고 SECS 연결 구현을 생성하는 공급자 계약입니다.</para>
/// \endif
/// \if EN
/// <para>Defines a provider that creates SECS connection implementations without exposing vendor SDK types.</para>
/// \endif
/// </summary>
public interface ISecsCommunicationProvider
{
    /// <summary>
    /// \if KO
    /// <para>설정과 Registry 조회에 사용할 안정적인 공급자 키를 가져옵니다.</para>
    /// \endif
    /// \if EN
    /// <para>Gets the stable provider key used by configuration and registry lookup.</para>
    /// \endif
    /// </summary>
    string Key { get; }

    /// <summary>
    /// \if KO
    /// <para>지정한 설정에 대한 연결 인스턴스를 생성합니다.</para>
    /// \endif
    /// \if EN
    /// <para>Creates a connection instance for the specified options.</para>
    /// \endif
    /// </summary>
    /// <param name="options">
    /// \if KO
    /// <para>공급자, 역할 및 연결 모드 설정입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The provider, role, and connection-mode settings.</para>
    /// \endif
    /// </param>
    /// <returns>
    /// \if KO
    /// <para>생성된 공급자 독립 연결 계약입니다.</para>
    /// \endif
    /// \if EN
    /// <para>The created provider-independent connection contract.</para>
    /// \endif
    /// </returns>
    ISecsConnection CreateConnection(SecsConnectionOptions options);
}
