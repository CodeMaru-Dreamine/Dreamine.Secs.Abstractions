# Public API review

Review date: 2026-08-10. Baseline: 1.0.0 source. The complete exported-type inventory is in [PUBLIC_API.md](PUBLIC_API.md).

## Result

- Reference direction is `Secs.Abstractions -> Communication.Abstractions`; no implementation project or cycle is present.
- Public models copy caller-owned arrays and expose `ReadOnlyMemory<T>` or `IReadOnlyList<T>`.
- Options now reject undefined numeric connection-mode and role enum values. This is a non-breaking validation correction; declared `Unspecified` values remain constructible and are rejected by the concrete session where a runnable mode/role is required.
- No public signature, binary surface, package ID, or target framework changed in this hardening pass.

## Next-version proposals

| Classification | Proposal | Reason |
|---|---|---|
| Source- and binary-breaking | Add an explicit HSMS session contract rather than expanding `ISecsConnection` | Select, linktest, messaging, state, and async disposal are concrete-session capabilities today. |
| Source- and binary-breaking | Replace synchronous message events with an async handler boundary | Event handlers cannot express backpressure or cancellation. Keep the current event for 1.x compatibility. |
| Non-breaking candidate | Add factory helpers for validated Active/Passive options | Reduces invalid intermediate option states without removing the mutable options object. |

Thread safety belongs to concrete implementations unless stated by a model. Diagnostics subscribers must be isolated by implementations because `ISecsDiagnosticSink.Emit` is synchronous.
