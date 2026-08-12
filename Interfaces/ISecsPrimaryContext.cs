using Dreamine.Secs.Abstractions.Model;

namespace Dreamine.Secs.Abstractions.Interfaces;

/// <summary>
/// \if KO <para>dispatcher가 하나의 inbound Primary handler에 제공하는 immutable 처리 문맥입니다.</para> \endif
/// \if EN <para>Defines the immutable processing context supplied to one inbound-primary handler.</para> \endif
/// </summary>
public interface ISecsPrimaryContext
{
    /// <summary>\if KO Primary를 수신한 session과 connection epoch의 identity를 가져옵니다. inbound 문맥의 epoch는 양수입니다. \endif \if EN Gets the session and positive connection-epoch identity on which the primary was received. \endif</summary>
    SecsConnectionIdentity ConnectionIdentity { get; }

    /// <summary>\if KO 수신된 immutable Primary message를 가져옵니다. \endif \if EN Gets the received immutable primary message. \endif</summary>
    SecsMessage Primary { get; }

    /// <summary>\if KO 실제 inbound W-bit가 true이고 exact 등록도 W1일 때만 one-shot 정상 Secondary가 허용되는지 가져옵니다. exact W0, actual/registered W mismatch 및 fallback은 false입니다. \endif \if EN Gets whether a one-shot normal secondary is permitted, which requires both an actual inbound W-bit of true and an exact W1 registration. Exact W0, actual/registered W mismatch, and fallback contexts return false. \endif</summary>
    bool CanReply { get; }

    /// <summary>\if KO <see cref="CanReply"/>가 true일 때 동일한 Session ID, Stream, System Bytes와 등록된 정상 Secondary Function으로 응답합니다. false일 때는 추측 응답을 만들지 않고 실패합니다. 첫 응답 시도는 송신 성공, 실패 또는 취소와 관계없이 ownership을 영구 소비하므로 이후 호출은 실패합니다. \endif \if EN When <see cref="CanReply"/> is true, replies with the same session ID, stream, and System Bytes and the registered normal secondary function. When false, it fails without constructing a guessed reply. The first reply attempt permanently consumes ownership regardless of send success, failure, or cancellation, so later calls fail. \endif</summary>
    /// <param name="item">\if KO 선택적 Secondary body입니다. \endif \if EN The optional secondary body. \endif</param>
    /// <param name="cancellationToken">\if KO 취소 토큰입니다. \endif \if EN The cancellation token. \endif</param>
    ValueTask ReplyAsync(SecsItem? item = null, CancellationToken cancellationToken = default);
}
