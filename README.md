# Dreamine.Secs.Abstractions

[![CI](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/actions/workflows/ci.yml/badge.svg)](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/actions/workflows/ci.yml)
[![Quality Gate](https://sonarcloud.io/api/project_badges/measure?project=CodeMaru-Dreamine_Dreamine.Secs.Abstractions&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=CodeMaru-Dreamine_Dreamine.Secs.Abstractions) [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=CodeMaru-Dreamine_Dreamine.Secs.Abstractions&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=CodeMaru-Dreamine_Dreamine.Secs.Abstractions) [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=CodeMaru-Dreamine_Dreamine.Secs.Abstractions&metric=coverage)](https://sonarcloud.io/summary/new_code?id=CodeMaru-Dreamine_Dreamine.Secs.Abstractions)
[![License: MIT](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/blob/main/LICENSE) [![.NET 8](https://img.shields.io/badge/.NET-8-512BD4.svg?logo=dotnet)](https://dotnet.microsoft.com/download/dotnet/8.0) [![NuGet](https://img.shields.io/nuget/v/Dreamine.Secs.Abstractions?logo=nuget&label=nuget)](https://www.nuget.org/packages/Dreamine.Secs.Abstractions) [![NuGet downloads](https://img.shields.io/nuget/dt/Dreamine.Secs.Abstractions?logo=nuget&label=downloads)](https://www.nuget.org/packages/Dreamine.Secs.Abstractions) [![Docs](https://img.shields.io/badge/Docs-README-2496ED.svg)](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions#readme)

Dreamine.Secs.Abstractions is the provider-independent domain and contract layer for Dreamine SECS-II and HSMS-SS communication.

[➡️ 한국어 문서 보기](https://github.com/CodeMaru-Dreamine/Dreamine.Secs.Abstractions/blob/main/README_KO.md)

## Install

```powershell
dotnet add package Dreamine.Secs.Abstractions
```

Choose this package when implementing a SECS provider, adapter, or application boundary without taking a dependency on the native TCP runtime. Applications that need a ready-to-run SECS-II/HSMS implementation should start with [`Dreamine.Secs.Com`](https://www.nuget.org/packages/Dreamine.Secs.Com).

## Implemented scope

- Immutable `SecsMessage`, typed Session ID, Stream, Function, W-bit, and System Bytes values.
- Typed SECS-II items: List, Binary, Boolean, ASCII, raw JIS-8, I1/I2/I4/I8, U1/U2/U4/U8, F4, and F8.
- HSMS ten-byte header, data/control messages, SType/status/reject/state/timer models.
- Structured validation results and protocol, decode, state, transaction-timeout, and HSMS-timer exceptions. Decode exceptions and diagnostics can retain typed HSMS header context when a complete header was available.
- Configurable T3/T5/T6/T7/T8 options plus frame, message, nesting-depth, and per-list child-count limits.
- Opt-in complete-frame wire-observation contracts with bounded capture, drop accounting, connection epochs, and a logging-framework-neutral diagnostic sink.
- Provider and connection lifecycle contracts based on `Dreamine.Communication.Abstractions`.
- Additive provider-neutral `ISecsMessageSession` / `ISecsMessageSessionProvider` contracts with typed identity and state, automatically allocated W0/W1 sends, and normal `SecsDialogueDefinition` correlation.
- A bounded asynchronous inbound-Primary dispatcher with exact S/F precedence, fallback handling, explicit drop accounting, and one-shot epoch-bound replies.

The model intentionally avoids `object`, `dynamic`, and string-based type discrimination. Caller-owned arrays are copied at item construction; values are exposed as read-only memory.

## Wire-observation boundary

Wire observation is disabled by default. When enabled by an implementation, `IHsmsWireObservationSource` exposes one pull consumer and never requires the protocol path to invoke or wait for that consumer. An observation describes a complete frame including its four-byte length prefix, but `CapturedBytes` can be a truncated retained prefix and sequence gaps indicate dropped observations. Captured bytes can contain sensitive application payload and must be protected accordingly. Partial frames are not observations.

## Message-session boundary

The original `ISecsConnection` and `ISecsCommunicationProvider` remain unchanged; the typed message-session interfaces are parallel additive contracts. Exact dispatcher registration wins over fallback, and fallback wins over the legacy message event. Exact W-bit mismatch is still claimed but cannot reply. A full or stopping bounded queue drops the newest already-claimed Primary without falling through. The first reply attempt consumes ownership even if sending fails or is canceled.

Endpoint, Active/Passive mode, Session ID, role, timers, limits, `AutoReconnect`, dispatcher settings, and wire-observation settings are construction snapshots. Change them by creating a new session; per-call cancellation affects only that operation. Role selects upper application/responder policy and does not alter the HSMS state machine.

## Standard boundary

The implemented wire rules were checked against locally held SEMI E5-0813 and E37-0413 documents. This package does not claim current-revision conformance, certification, or vendor interoperability. SEMI source documents are not distributed with the repository.

Deferred work includes SECS-I, two-byte-character items, detailed Stream 9 bodies, SML, GEM, GEM300, vendor SDK providers, and UI.

## Dependency boundary

    Dreamine.Secs.Abstractions
        -> Dreamine.Communication.Abstractions

This project never references `Dreamine.Secs.Com` or a vendor implementation.

## License

MIT.
