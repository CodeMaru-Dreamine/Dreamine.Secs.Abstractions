using Dreamine.Secs.Abstractions.Options;

namespace Dreamine.Secs.Abstractions.Interfaces;

/// <summary>
/// \if KO <para>기존 연결 공급자 경계를 유지하면서 typed SECS message session을 생성하는 추가 공급자 계약입니다.</para> \endif
/// \if EN <para>Adds typed SECS message-session creation while preserving the existing connection-provider boundary.</para> \endif
/// </summary>
public interface ISecsMessageSessionProvider : ISecsCommunicationProvider
{
    /// <summary>\if KO provider-neutral 설정으로 typed message session을 생성합니다. \endif \if EN Creates a typed message session from provider-neutral settings. \endif</summary>
    /// <param name="options">\if KO 공급자, 역할 및 연결 모드 설정입니다. \endif \if EN Provider, role, and connection-mode settings. \endif</param>
    /// <returns>\if KO 생성된 typed message session입니다. \endif \if EN The created typed message session. \endif</returns>
    ISecsMessageSession CreateSession(SecsConnectionOptions options);
}
