# Dreamine.Secs.Abstractions

Dreamine.Secs.Abstractions는 Dreamine SECS-II 및 HSMS-SS 통신의 공급자 독립 도메인/계약 계층입니다.

[➡️ English Version](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/blob/main/README.md)

## 구현 범위

- 불변 `SecsMessage`와 Session ID, Stream, Function, W-bit, System Bytes 값 형식
- List, Binary, Boolean, ASCII, JIS-8 원시 바이트, I1/I2/I4/I8, U1/U2/U4/U8, F4/F8 SECS-II item
- HSMS 10바이트 Header, Data/Control Message, SType/Status/Reject/State/Timer 모델
- 구조화된 검증 결과와 Protocol/Decode/State/Transaction Timeout/HSMS Timer 예외. 완전한 Header를 확보한 경우 Decode 예외와 진단에 typed HSMS Header 문맥을 보존
- 설정 가능한 T3/T5/T6/T7/T8 및 Frame/Message/List 중첩 깊이/List별 child 수 제한
- 제한된 capture, drop 계수, 연결 epoch를 제공하는 opt-in 완전 Frame wire observation 계약과 특정 Logging 구현과 분리된 진단 sink
- `Dreamine.Communication.Abstractions` 기반 공급자/연결 생명주기 계약
- typed identity/state, System Bytes 자동 할당 W0/W1 송신 및 정상 `SecsDialogueDefinition` 상관관계를 제공하는 추가 provider-neutral `ISecsMessageSession` / `ISecsMessageSessionProvider` 계약
- exact S/F 우선, fallback 처리, 명시적 drop 계수 및 connection epoch에 결합된 one-shot 응답을 제공하는 bounded 비동기 inbound Primary dispatcher

핵심 모델은 `object`, `dynamic`, 문자열 기반 형식 판별을 사용하지 않습니다. Item 생성 시 호출자 배열을 복사하고 값을 읽기 전용 메모리로 노출합니다.

## Wire observation 경계

Wire observation은 기본적으로 비활성입니다. 구현에서 활성화하면 `IHsmsWireObservationSource`는 하나의 pull consumer를 노출하며 protocol 경로가 consumer를 호출하거나 기다리게 하지 않습니다. Observation은 4바이트 길이 prefix를 포함한 완전한 Frame을 설명하지만 `CapturedBytes`는 잘린 앞부분일 수 있고 sequence 번호의 공백은 drop을 뜻합니다. Capture된 바이트에는 민감한 application payload가 포함될 수 있으므로 그에 맞게 보호해야 합니다. 부분 Frame은 observation으로 보고하지 않습니다.

## Message session 경계

기존 `ISecsConnection`과 `ISecsCommunicationProvider`는 변경하지 않았고 typed message session interface는 병렬로 추가한 계약입니다. Dispatcher는 exact 등록, fallback, legacy message event 순서로 claim합니다. exact W-bit 불일치도 claim하지만 응답할 수 없습니다. Bounded queue가 가득 찼거나 종료 중이면 이미 claim한 newest Primary를 drop하며 다른 경로로 넘기지 않습니다. 첫 응답 시도는 송신 실패나 취소 여부와 관계없이 ownership을 소비합니다.

Endpoint, Active/Passive mode, Session ID, role, timer, limit, `AutoReconnect`, dispatcher 및 wire observation 설정은 session 생성 시 snapshot입니다. 변경하려면 새 session을 만들고, 호출별 cancellation은 해당 작업에만 적용합니다. Role은 상위 application/responder policy이며 HSMS 상태 머신을 바꾸지 않습니다.

## 표준 경계

Wire 규칙은 로컬 보유 SEMI E5-0813 및 E37-0413 문서로 확인했습니다. 최신 Revision 준수, 인증 또는 벤더 상호운용성을 주장하지 않으며 SEMI 원문은 저장소에 포함하지 않습니다.

SECS-I, 2-byte character item, 상세 Stream 9 본문, SML, GEM, GEM300, 벤더 SDK 공급자와 UI는 제외 범위입니다.

## 의존성 경계

    Dreamine.Secs.Abstractions
        -> Dreamine.Communication.Abstractions

이 프로젝트는 `Dreamine.Secs.Com` 또는 벤더 구현을 참조하지 않습니다.

## 라이선스

MIT.
