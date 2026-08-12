using Dreamine.Secs.Abstractions.Diagnostics;
using Dreamine.Secs.Abstractions.Hsms;
using Dreamine.Secs.Abstractions.Model;

namespace Dreamine.Secs.Abstractions.Interfaces;

/// <summary>
/// \if KO
/// <para>provider와 무관한 typed SECS-II/HSMS message session 계약입니다.</para>
/// <para>저수준 expert API와 System Bytes를 자동 할당하는 안전한 W0/W1 API를 함께 제공합니다.</para>
/// \endif
/// \if EN
/// <para>Defines a provider-independent typed SECS-II/HSMS message session.</para>
/// <para>Provides both low-level expert APIs and safe W0/W1 APIs that allocate System Bytes automatically.</para>
/// \endif
/// </summary>
public interface ISecsMessageSession : ISecsConnection, IHsmsWireObservationSource
{
    /// <summary>\if KO session instance와 현재 연결 epoch의 typed identity를 가져옵니다. \endif \if EN Gets the typed identity of the session instance and current connection epoch. \endif</summary>
    SecsConnectionIdentity ConnectionIdentity { get; }

    /// <summary>\if KO 현재 HSMS protocol 상태를 가져옵니다. \endif \if EN Gets the current HSMS protocol state. \endif</summary>
    HsmsConnectionState HsmsState { get; }

    /// <summary>\if KO inbound Primary를 정확한 S/F 또는 fallback handler로 전달하는 bounded dispatcher를 가져옵니다. \endif \if EN Gets the bounded dispatcher that routes inbound primaries to exact S/F or fallback handlers. \endif</summary>
    ISecsPrimaryDispatcher PrimaryDispatcher { get; }

    /// <summary>\if KO 선택된 session에서 전달된 SECS message에 대해 발생합니다. \endif \if EN Raised for a SECS message delivered by a selected session. \endif</summary>
    event EventHandler<SecsMessage>? MessageReceived;

    /// <summary>\if KO session의 typed bounded 진단 이벤트에 대해 발생합니다. \endif \if EN Raised for a typed, bounded session diagnostic event. \endif</summary>
    event EventHandler<SecsDiagnosticEvent>? DiagnosticReceived;

    /// <summary>\if KO TCP lifecycle 또는 HSMS protocol 상태가 변경될 때 previous/current typed snapshot과 함께 발생합니다. \endif \if EN Raised with a typed previous/current snapshot when the TCP lifecycle or HSMS protocol state changes. \endif</summary>
    event EventHandler<SecsSessionStateChangedEventArgs>? StateChanged;

    /// <summary>\if KO HSMS Select transaction을 수행합니다. \endif \if EN Performs an HSMS Select transaction. \endif</summary>
    Task SelectAsync(CancellationToken cancellationToken = default);

    /// <summary>\if KO HSMS Deselect transaction을 수행합니다. \endif \if EN Performs an HSMS Deselect transaction. \endif</summary>
    Task DeselectAsync(CancellationToken cancellationToken = default);

    /// <summary>\if KO HSMS Linktest transaction을 수행합니다. \endif \if EN Performs an HSMS Linktest transaction. \endif</summary>
    Task LinktestAsync(CancellationToken cancellationToken = default);

    /// <summary>\if KO HSMS Separate request를 보내고 현재 연결을 종료합니다. \endif \if EN Sends an HSMS Separate request and closes the current connection. \endif</summary>
    Task SeparateAsync(CancellationToken cancellationToken = default);

    /// <summary>\if KO 열린 transaction과 충돌하지 않는 System Bytes를 할당하는 expert API입니다. \endif \if EN Expert API that allocates System Bytes not colliding with an open transaction. \endif</summary>
    SecsSystemBytes AllocateSystemBytes();

    /// <summary>\if KO 응답 transaction을 열지 않는 완성된 message를 보내는 expert API입니다. \endif \if EN Expert API that sends a complete message without opening a reply transaction. \endif</summary>
    Task SendAsync(SecsMessage message, CancellationToken cancellationToken = default);

    /// <summary>\if KO 완성된 W-bit Primary를 보내고 정상 Secondary를 기다리는 expert API입니다. \endif \if EN Expert API that sends a complete W-bit primary and waits for its normal secondary. \endif</summary>
    Task<SecsMessage> SendPrimaryAsync(SecsMessage message, CancellationToken cancellationToken = default);

    /// <summary>\if KO 설정된 Session ID와 자동 할당된 System Bytes로 안전한 W0 Primary를 보냅니다. \endif \if EN Sends a safe W0 primary with the configured session ID and automatically allocated System Bytes. \endif</summary>
    /// <param name="stream">\if KO 1부터 127까지의 Stream입니다. \endif \if EN The stream from 1 through 127. \endif</param>
    /// <param name="function">\if KO 0이 아닌 홀수 Primary Function입니다. \endif \if EN The nonzero odd primary function. \endif</param>
    /// <param name="item">\if KO 선택적 message body입니다. \endif \if EN The optional message body. \endif</param>
    /// <param name="cancellationToken">\if KO 취소 토큰입니다. \endif \if EN The cancellation token. \endif</param>
    Task SendAsync(
        SecsStream stream,
        SecsFunction function,
        SecsItem? item = null,
        CancellationToken cancellationToken = default);

    /// <summary>\if KO W1 대화 정의와 자동 할당된 System Bytes로 Primary를 보내고 선언된 정상 Secondary를 기다립니다. \endif \if EN Sends a primary using a W1 dialogue and automatically allocated System Bytes, then waits for the declared normal secondary. \endif</summary>
    /// <param name="dialogue">\if KO 인접 정상 Secondary가 선언된 W1 대화입니다. \endif \if EN A W1 dialogue declaring its adjacent normal secondary. \endif</param>
    /// <param name="item">\if KO 선택적 Primary body입니다. \endif \if EN The optional primary body. \endif</param>
    /// <param name="cancellationToken">\if KO 취소 토큰입니다. \endif \if EN The cancellation token. \endif</param>
    Task<SecsMessage> RequestAsync(
        SecsDialogueDefinition dialogue,
        SecsItem? item = null,
        CancellationToken cancellationToken = default);
}
