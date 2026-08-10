# Dreamine.Secs.Abstractions

Dreamine.Secs.Abstractions는 Dreamine SECS-II 및 HSMS-SS 통신의 공급자 독립 도메인/계약 계층입니다.

[➡️ English Version](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/blob/main/README.md)

## 구현 범위

- 불변 `SecsMessage`와 Session ID, Stream, Function, W-bit, System Bytes 값 형식
- List, Binary, Boolean, ASCII, JIS-8 원시 바이트, I1/I2/I4/I8, U1/U2/U4/U8, F4/F8 SECS-II item
- HSMS 10바이트 Header, Data/Control Message, SType/Status/Reject/State/Timer 모델
- 구조화된 검증 결과와 Protocol/Decode/State/Transaction Timeout/HSMS Timer 예외
- 설정 가능한 T3/T5/T6/T7/T8 및 특정 Logging 구현과 분리된 진단 sink
- `Dreamine.Communication.Abstractions` 기반 공급자/연결 생명주기 계약

핵심 모델은 `object`, `dynamic`, 문자열 기반 형식 판별을 사용하지 않습니다. Item 생성 시 호출자 배열을 복사하고 값을 읽기 전용 메모리로 노출합니다.

## 표준 경계

Wire 규칙은 로컬 보유 SEMI E5-0813 및 E37-0413 문서로 확인했습니다. 최신 Revision 준수, 인증 또는 벤더 상호운용성을 주장하지 않으며 SEMI 원문은 저장소에 포함하지 않습니다.

SECS-I, 2-byte character item, 상세 Stream 9 본문, SML, GEM, GEM300, 벤더 SDK 공급자와 UI는 제외 범위입니다.

## 의존성 경계

    Dreamine.Secs.Abstractions
        -> Dreamine.Communication.Abstractions

이 프로젝트는 `Dreamine.Secs.Com` 또는 벤더 구현을 참조하지 않습니다.

## 라이선스

MIT.
