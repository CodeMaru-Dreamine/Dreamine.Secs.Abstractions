# Public API review

Review date: 2026-08-12. Baseline package version: 1.0.0 source. The complete exported-type inventory is in [PUBLIC_API.md](PUBLIC_API.md).

## Result

- Reference direction is `Secs.Abstractions -> Communication.Abstractions`; no implementation project or cycle is present.
- Public models copy caller-owned arrays and expose `ReadOnlyMemory<T>` or `IReadOnlyList<T>`.
- Options reject undefined numeric connection-mode and role enum values. Declared `Unspecified` values remain constructible and are rejected by the concrete session where a runnable mode/role is required.
- This hardening pass made additive public API changes. In addition to the four wire-observation types, limits, and typed diagnostic context, it added `ISecsMessageSession`, `ISecsMessageSessionProvider`, `ISecsPrimaryContext`, `ISecsPrimaryDispatcher`, `SecsDialogueDefinition`, `SecsConnectionIdentity`, `SecsPrimaryDispatcherOptions`, `SecsSessionStateChangedEventArgs`, and `HsmsSessionOptions.PrimaryDispatcher`.
- Existing constructors, `ISecsConnection`, `ISecsCommunicationProvider`, package IDs, and target frameworks were retained. No existing public member was removed or made required.
- Wire observation is disabled by default. The contract exposes a bounded, single-reader pull stream, explicit drop accounting, capture truncation, and a warning that captured bytes can contain sensitive application payload.
- The new message-session/provider contracts are parallel, additive contracts. The original `ISecsConnection.CreateConnection` shape and `ISecsCommunicationProvider` surface are unchanged.

Reflection over the Release assembly records **64 exported types**, up from 52 at the 1.0.0 baseline. Because locally generated packages still use version `1.0.0`, a new `Dreamine.Secs.Com` binary must not be distributed with an older cached `Dreamine.Secs.Abstractions.1.0.0`. Before publication, both packages need a matching new version and an isolated-cache package-consumer smoke test. This review does not change a package version or publish a package.

## Implemented message-session contract

- `ISecsMessageSession` provides provider-neutral Select/Deselect/Linktest/Separate, expert send methods, automatically allocated W0/W1 methods, typed identity/state/diagnostics, and the bounded primary dispatcher.
- `SecsDialogueDefinition` models a normal W0 dialogue or a normal W1 dialogue whose Secondary is the adjacent even function (`P + 1`). Function 0 remains a special transaction terminator accepted by the low-level correlation path, not a normal Secondary that can be declared in this type.
- Dispatcher ownership is exact S/F first, fallback second, and legacy `MessageReceived` only when neither claims the Primary. Exact W-bit mismatch remains claimed but cannot reply and never falls through.
- Accepted claimed Primaries enter a bounded FIFO. A full or stopping dispatcher drops the newest already-claimed Primary, increments `DroppedPrimaryCount`, and does not fall through. Completion order is not promised when `MaximumConcurrency` is greater than one.
- A reply is permitted only for actual W1 plus an exact W1 registration. The first `ReplyAsync` attempt consumes its one-shot ownership regardless of success, failure, or cancellation; the reply remains bound to the source session, Session ID, Stream, System Bytes, and connection epoch.
- `StateChanged` exposes typed previous/current TCP and HSMS state snapshots. The implementation must raise callbacks outside internal state locks so reentrant readers do not deadlock.

## Configuration classification

- Endpoint, Active/Passive mode, Session ID, role, timers, limits, `AutoReconnect`, dispatcher options, and wire-observation options are construction snapshots. They are not a live-mutation contract; create a new session for a changed value.
- Per-call cancellation cancels only the current lifecycle, control, send, request, or reply operation. It does not rewrite session configuration.
- `SecsRole` is upper application/responder policy. It does not alter the HSMS state machine, whose TCP direction is controlled independently by Active/Passive mode.
- A later profile-application layer must validate a proposed profile, diff it from the active snapshot, and recreate the session when any construction setting changes.

## Next-version proposals

| Classification | Proposal | Reason |
|---|---|---|
| Non-breaking candidate | Add factory helpers for validated Active/Passive options | Reduces invalid intermediate option states without changing the snapshot contract. |
| Additive candidate | Add opt-in session metrics snapshots beyond the existing drop counters | Enables diagnostics without coupling the contract to a logging/metrics framework. |

Thread safety belongs to concrete implementations unless stated by a model. Diagnostics subscribers must be isolated by implementations because `ISecsDiagnosticSink.Emit` is synchronous.
