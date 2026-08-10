# Dreamine.Secs.Abstractions

Dreamine.Secs.Abstractions is the provider-independent domain and contract layer for Dreamine SECS-II and HSMS-SS communication.

[➡️ 한국어 문서 보기](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/blob/main/README_KO.md)

## Implemented scope

- Immutable `SecsMessage`, typed Session ID, Stream, Function, W-bit, and System Bytes values.
- Typed SECS-II items: List, Binary, Boolean, ASCII, raw JIS-8, I1/I2/I4/I8, U1/U2/U4/U8, F4, and F8.
- HSMS ten-byte header, data/control messages, SType/status/reject/state/timer models.
- Structured validation results and protocol, decode, state, transaction-timeout, and HSMS-timer exceptions.
- Configurable T3/T5/T6/T7/T8 options and a logging-framework-neutral diagnostic sink.
- Provider and connection lifecycle contracts based on `Dreamine.Communication.Abstractions`.

The model intentionally avoids `object`, `dynamic`, and string-based type discrimination. Caller-owned arrays are copied at item construction; values are exposed as read-only memory.

## Standard boundary

The implemented wire rules were checked against locally held SEMI E5-0813 and E37-0413 documents. This package does not claim current-revision conformance, certification, or vendor interoperability. SEMI source documents are not distributed with the repository.

Deferred work includes SECS-I, two-byte-character items, detailed Stream 9 bodies, SML, GEM, GEM300, vendor SDK providers, and UI.

## Dependency boundary

    Dreamine.Secs.Abstractions
        -> Dreamine.Communication.Abstractions

This project never references `Dreamine.Secs.Com` or a vendor implementation.

## License

MIT.
