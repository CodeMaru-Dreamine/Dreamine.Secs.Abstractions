using Dreamine.Secs.Abstractions.Model;

namespace Dreamine.Secs.Abstractions.Interfaces;

/// <summary>
/// \if KO
/// <para>inbound Primary를 exact S/F handler 우선, fallback handler 차선으로 전달하는 bounded dispatcher 계약입니다.</para>
/// <para>exact S/F 등록은 실제 W-bit가 등록 정의와 달라도 message를 claim하고 handler에 전달합니다. 이 mismatch는 fallback 또는 legacy message event로 흘려보내지 않으며 추측 응답을 생성하지 않습니다.</para>
/// <para>응답 ownership은 실제 inbound W-bit가 true이고 exact 등록도 W1인 경우에만 부여됩니다. exact W0, W-bit mismatch 및 fallback 문맥은 응답할 수 없습니다.</para>
/// <para>accepted Primary는 bounded queue에 FIFO로 들어갑니다. queue가 가득 차면 dispatcher가 이미 claim한 newest Primary를 drop하며 fallback 또는 legacy message event로 넘기지 않습니다. 병렬 실행 수가 1보다 크면 handler 완료 순서는 보장되지 않습니다.</para>
/// \endif
/// \if EN
/// <para>Defines a bounded dispatcher that routes inbound primaries to an exact S/F handler first and a fallback handler second.</para>
/// <para>An exact S/F registration claims and delivers a message even when its actual W-bit differs from the registered definition. A mismatch never falls through to fallback or the legacy message event and never produces a guessed reply.</para>
/// <para>Reply ownership is granted only when the actual inbound W-bit is true and the exact registration is also W1. Exact W0, W-bit mismatch, and fallback contexts cannot reply.</para>
/// <para>Accepted primaries enter the bounded queue in FIFO order. When the queue is full, the dispatcher drops the newest primary that it already claimed; it never falls through to fallback or the legacy message event. Handler completion order is not guaranteed when concurrency is greater than one.</para>
/// \endif
/// </summary>
public interface ISecsPrimaryDispatcher
{
    /// <summary>\if KO bounded queue 포화 또는 dispatcher 종료로 인해 claim된 뒤 enqueue되지 못한 inbound Primary 수를 가져옵니다. \endif \if EN Gets the number of inbound primaries claimed but not enqueued because the bounded queue was full or the dispatcher was stopping. \endif</summary>
    long DroppedPrimaryCount { get; }

    /// <summary>\if KO exact Stream/Primary Function handler를 등록합니다. 반환된 handle을 해제하면 등록이 제거됩니다. \endif \if EN Registers an exact stream/primary-function handler. Disposing the returned handle removes the registration. \endif</summary>
    /// <param name="dialogue">\if KO W0 또는 정상 W1 대화 정의입니다. \endif \if EN A W0 or normal W1 dialogue definition. \endif</param>
    /// <param name="handler">\if KO 비동기 Primary handler입니다. \endif \if EN The asynchronous primary handler. \endif</param>
    /// <returns>\if KO 등록 수명 handle입니다. \endif \if EN A registration-lifetime handle. \endif</returns>
    IDisposable Register(
        SecsDialogueDefinition dialogue,
        Func<ISecsPrimaryContext, CancellationToken, ValueTask> handler);

    /// <summary>\if KO exact S/F 등록이 없는 Primary를 claim하는 fallback handler를 등록합니다. fallback 문맥은 실제 W-bit와 관계없이 응답할 수 없습니다. \endif \if EN Registers the fallback handler that claims primaries without an exact S/F registration. A fallback context cannot reply regardless of the actual W-bit. \endif</summary>
    /// <param name="handler">\if KO 비동기 fallback handler입니다. \endif \if EN The asynchronous fallback handler. \endif</param>
    /// <returns>\if KO 등록 수명 handle입니다. \endif \if EN A registration-lifetime handle. \endif</returns>
    IDisposable RegisterFallback(Func<ISecsPrimaryContext, CancellationToken, ValueTask> handler);
}
