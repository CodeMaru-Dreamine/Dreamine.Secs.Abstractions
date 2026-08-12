# Public API Inventory

Assembly: `Dreamine.Secs.Abstractions`

This inventory is generated from the compiled Release assembly. It is an audit artifact, not an additional compatibility promise.

Exported types: **66**

## Types

### `public interface Dreamine.Secs.Abstractions.Codecs.ISecsItemCodec`

- `Dreamine.Secs.Abstractions.Model.SecsItem Decode(System.ReadOnlyMemory<System.Byte> data)`
- `Dreamine.Secs.Abstractions.Validation.SecsValidationResult Validate(System.ReadOnlyMemory<System.Byte> data)`
- `System.Byte[] Encode(Dreamine.Secs.Abstractions.Model.SecsItem item)`

### `public interface Dreamine.Secs.Abstractions.Diagnostics.ISecsDiagnosticSink`

- `System.Void Emit(Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticEvent diagnosticEvent)`

### `public sealed class Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticEvent`

- `Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind Kind { get; }`
- `SecsDiagnosticEvent(Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind kind, System.String message, System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState> state, System.Nullable<System.Int32> frameLength)`
- `SecsDiagnosticEvent(Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind kind, System.String message, System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState> state, System.Nullable<System.Int32> frameLength, System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsHeader> hsmsHeader)`
- `System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState> State { get; }`
- `System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsHeader> HsmsHeader { get; }`
- `System.Nullable<System.Int32> FrameLength { get; }`
- `System.String Message { get; }`

### `public enum Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind`

- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind ApplicationError = 11`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind ConnectionAttempt = 0`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind ConnectionClosed = 2`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind ConnectionEstablished = 1`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind FrameReceived = 5`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind FrameSent = 4`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind PrimarySent = 6`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind ProtocolError = 10`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind Reject = 9`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind SecondaryReceived = 7`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind StateChanged = 3`
- `const Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticKind Timeout = 8`

### `public enum Dreamine.Secs.Abstractions.Enums.SecsConnectionMode`

- `const Dreamine.Secs.Abstractions.Enums.SecsConnectionMode Active = 1`
- `const Dreamine.Secs.Abstractions.Enums.SecsConnectionMode Passive = 2`
- `const Dreamine.Secs.Abstractions.Enums.SecsConnectionMode Unspecified = 0`

### `public enum Dreamine.Secs.Abstractions.Enums.SecsRole`

- `const Dreamine.Secs.Abstractions.Enums.SecsRole Equipment = 2`
- `const Dreamine.Secs.Abstractions.Enums.SecsRole Host = 1`
- `const Dreamine.Secs.Abstractions.Enums.SecsRole Unspecified = 0`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState ConnectedNotSelected = 1`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState NotConnected = 0`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState Selected = 2`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsControlMessage`

- `Dreamine.Secs.Abstractions.Hsms.HsmsSType SType { get; }`
- `HsmsControlMessage(Dreamine.Secs.Abstractions.Hsms.HsmsHeader header)`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsDataMessage`

- `Dreamine.Secs.Abstractions.Model.SecsMessage SecsMessage { get; }`
- `HsmsDataMessage(Dreamine.Secs.Abstractions.Model.SecsMessage message)`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsDeselectStatus`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsDeselectStatus Busy = 2`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsDeselectStatus NotEstablished = 1`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsDeselectStatus Success = 0`

### `public struct Dreamine.Secs.Abstractions.Hsms.HsmsHeader`

- `Dreamine.Secs.Abstractions.Hsms.HsmsHeader CreateControl(Dreamine.Secs.Abstractions.Hsms.HsmsSType sType, Dreamine.Secs.Abstractions.Model.SecsSystemBytes systemBytes, System.Byte headerByte2, System.Byte headerByte3, System.UInt16 sessionId)`
- `Dreamine.Secs.Abstractions.Hsms.HsmsHeader CreateData(Dreamine.Secs.Abstractions.Model.SecsMessage message)`
- `Dreamine.Secs.Abstractions.Model.SecsSystemBytes SystemBytes { get; }`
- `HsmsHeader(System.UInt16 sessionId, System.Byte headerByte2, System.Byte headerByte3, System.Byte pType, System.Byte sType, Dreamine.Secs.Abstractions.Model.SecsSystemBytes systemBytes)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Hsms.HsmsHeader other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Boolean IsData { get; }`
- `System.Boolean ReplyExpected { get; }`
- `System.Byte Function { get; }`
- `System.Byte HeaderByte2 { get; }`
- `System.Byte HeaderByte3 { get; }`
- `System.Byte PType { get; }`
- `System.Byte SType { get; }`
- `System.Byte Stream { get; }`
- `System.Int32 GetHashCode()`
- `System.String ToString()`
- `System.UInt16 SessionId { get; }`

### `public abstract class Dreamine.Secs.Abstractions.Hsms.HsmsMessage`

- `Dreamine.Secs.Abstractions.Hsms.HsmsHeader Header { get; }`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsRejectReason`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsRejectReason NotSelected = 4`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsRejectReason TransactionNotOpen = 3`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsRejectReason UnsupportedPType = 2`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsRejectReason UnsupportedSType = 1`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsSType`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType Data = 0`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType DeselectRequest = 3`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType DeselectResponse = 4`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType LinktestRequest = 5`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType LinktestResponse = 6`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType RejectRequest = 7`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType SelectRequest = 1`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType SelectResponse = 2`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSType SeparateRequest = 9`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsSelectStatus`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsSelectStatus AlreadyActive = 1`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSelectStatus Exhausted = 3`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSelectStatus NotReady = 2`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsSelectStatus Success = 0`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsSessionOptions`

- `Dreamine.Secs.Abstractions.Enums.SecsConnectionMode Mode { get; set; }`
- `Dreamine.Secs.Abstractions.Enums.SecsRole Role { get; set; }`
- `Dreamine.Secs.Abstractions.Hsms.HsmsTimerOptions Timers { get; set; }`
- `Dreamine.Secs.Abstractions.Hsms.HsmsWireObservationOptions WireObservation { get; set; }`
- `Dreamine.Secs.Abstractions.Model.SecsSessionId SessionId { get; set; }`
- `Dreamine.Secs.Abstractions.Options.SecsPrimaryDispatcherOptions PrimaryDispatcher { get; set; }`
- `HsmsSessionOptions()`
- `System.Boolean AutoReconnect { get; set; }`
- `System.Int32 MaximumFrameLength { get; set; }`
- `System.Int32 MaximumListItemCount { get; set; }`
- `System.Int32 MaximumNestingDepth { get; set; }`
- `System.Int32 Port { get; set; }`
- `System.Nullable<System.Int32> MaximumMessageLength { get; set; }`
- `System.String Host { get; set; }`
- `System.Void Validate()`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsTimerKind`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsTimerKind T3 = 0`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsTimerKind T5 = 1`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsTimerKind T6 = 2`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsTimerKind T7 = 3`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsTimerKind T8 = 4`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsTimerOptions`

- `HsmsTimerOptions()`
- `System.TimeSpan T3 { get; set; }`
- `System.TimeSpan T5 { get; set; }`
- `System.TimeSpan T6 { get; set; }`
- `System.TimeSpan T7 { get; set; }`
- `System.TimeSpan T8 { get; set; }`
- `System.Void Validate()`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode Excluded = 0`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode FullFrame = 2`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode HeaderOnly = 1`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureRule`

- `Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode Mode { get; }`
- `HsmsWireCaptureRule(System.Byte stream, System.Byte function, System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection> direction, Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode mode, System.Int32 maximumCapturedBytes)`
- `System.Byte Function { get; }`
- `System.Byte Stream { get; }`
- `System.Int32 MaximumCapturedBytes { get; }`
- `System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection> Direction { get; }`

### `public enum Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection`

- `const Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection Inbound = 0`
- `const Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection Outbound = 1`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsWireObservation`

- `Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection Direction { get; }`
- `HsmsWireObservation(System.Int64 sequenceNumber, System.Int64 connectionEpoch, System.DateTimeOffset observedAtUtc, Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection direction, System.Int32 actualByteCount, System.Int32 declaredFrameLength, System.ReadOnlyMemory<System.Byte> capturedBytes)`
- `HsmsWireObservation(System.Int64 sequenceNumber, System.Int64 connectionEpoch, System.DateTimeOffset observedAtUtc, Dreamine.Secs.Abstractions.Hsms.HsmsWireDirection direction, System.Int32 actualByteCount, System.Int32 declaredFrameLength, System.ReadOnlyMemory<System.Byte> capturedBytes, System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsHeader> header)`
- `System.Boolean IsCaptureTruncated { get; }`
- `System.DateTimeOffset ObservedAtUtc { get; }`
- `System.Int32 ActualByteCount { get; }`
- `System.Int32 DeclaredFrameLength { get; }`
- `System.Int64 ConnectionEpoch { get; }`
- `System.Int64 SequenceNumber { get; }`
- `System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsHeader> Header { get; }`
- `System.ReadOnlyMemory<System.Byte> CapturedBytes { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.HsmsWireObservationOptions`

- `Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureMode DefaultCaptureMode { get; set; }`
- `HsmsWireObservationOptions()`
- `System.Collections.Generic.IReadOnlyList<Dreamine.Secs.Abstractions.Hsms.HsmsWireCaptureRule> CaptureRules { get; set; }`
- `System.Int32 MaximumCapturedBytes { get; set; }`
- `System.Int32 QueueCapacity { get; set; }`
- `System.Void Validate()`
- `const System.Int64 MaximumRetainedPayloadBytes = 67108864`

### `public interface Dreamine.Secs.Abstractions.Hsms.IHsmsWireObservationSource`

- `System.Boolean IsWireObservationEnabled { get; }`
- `System.Collections.Generic.IAsyncEnumerable<Dreamine.Secs.Abstractions.Hsms.HsmsWireObservation> ReadWireObservationsAsync(System.Threading.CancellationToken cancellationToken)`
- `System.Int64 DroppedWireObservationCount { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Hsms.SecsSessionStateChangedEventArgs`

- `Dreamine.Communication.Abstractions.Enums.ConnectionState CurrentConnectionState { get; }`
- `Dreamine.Communication.Abstractions.Enums.ConnectionState PreviousConnectionState { get; }`
- `Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState CurrentHsmsState { get; }`
- `Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState PreviousHsmsState { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity ConnectionIdentity { get; }`
- `SecsSessionStateChangedEventArgs(Dreamine.Communication.Abstractions.Enums.ConnectionState previousConnectionState, Dreamine.Communication.Abstractions.Enums.ConnectionState currentConnectionState, Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState previousHsmsState, Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState currentHsmsState, Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity connectionIdentity)`

### `public interface Dreamine.Secs.Abstractions.Interfaces.ISecsCommunicationProvider`

- `Dreamine.Secs.Abstractions.Interfaces.ISecsConnection CreateConnection(Dreamine.Secs.Abstractions.Options.SecsConnectionOptions options)`
- `System.String Key { get; }`

### `public interface Dreamine.Secs.Abstractions.Interfaces.ISecsConnection`

- `System.String ProviderKey { get; }`

### `public interface Dreamine.Secs.Abstractions.Interfaces.ISecsMessageSession`

- `Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState HsmsState { get; }`
- `Dreamine.Secs.Abstractions.Interfaces.ISecsPrimaryDispatcher PrimaryDispatcher { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity ConnectionIdentity { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsSystemBytes AllocateSystemBytes()`
- `System.Threading.Tasks.Task DeselectAsync(System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task LinktestAsync(System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task SelectAsync(System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task SendAsync(Dreamine.Secs.Abstractions.Model.SecsMessage message, System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task SendAsync(Dreamine.Secs.Abstractions.Model.SecsStream stream, Dreamine.Secs.Abstractions.Model.SecsFunction function, Dreamine.Secs.Abstractions.Model.SecsItem item, System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task SeparateAsync(System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task<Dreamine.Secs.Abstractions.Model.SecsMessage> RequestAsync(Dreamine.Secs.Abstractions.Model.SecsDialogueDefinition dialogue, Dreamine.Secs.Abstractions.Model.SecsItem item, System.Threading.CancellationToken cancellationToken)`
- `System.Threading.Tasks.Task<Dreamine.Secs.Abstractions.Model.SecsMessage> SendPrimaryAsync(Dreamine.Secs.Abstractions.Model.SecsMessage message, System.Threading.CancellationToken cancellationToken)`
- `event System.EventHandler<Dreamine.Secs.Abstractions.Diagnostics.SecsDiagnosticEvent> DiagnosticReceived`
- `event System.EventHandler<Dreamine.Secs.Abstractions.Hsms.SecsSessionStateChangedEventArgs> StateChanged`
- `event System.EventHandler<Dreamine.Secs.Abstractions.Model.SecsMessage> MessageReceived`

### `public interface Dreamine.Secs.Abstractions.Interfaces.ISecsMessageSessionProvider`

- `Dreamine.Secs.Abstractions.Interfaces.ISecsMessageSession CreateSession(Dreamine.Secs.Abstractions.Options.SecsConnectionOptions options)`

### `public interface Dreamine.Secs.Abstractions.Interfaces.ISecsPrimaryContext`

- `Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity ConnectionIdentity { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsMessage Primary { get; }`
- `System.Boolean CanReply { get; }`
- `System.Threading.Tasks.ValueTask ReplyAsync(Dreamine.Secs.Abstractions.Model.SecsItem item, System.Threading.CancellationToken cancellationToken)`

### `public interface Dreamine.Secs.Abstractions.Interfaces.ISecsPrimaryDispatcher`

- `System.IDisposable Register(Dreamine.Secs.Abstractions.Model.SecsDialogueDefinition dialogue, System.Func<Dreamine.Secs.Abstractions.Interfaces.ISecsPrimaryContext, System.Threading.CancellationToken, System.Threading.Tasks.ValueTask> handler)`
- `System.IDisposable RegisterFallback(System.Func<Dreamine.Secs.Abstractions.Interfaces.ISecsPrimaryContext, System.Threading.CancellationToken, System.Threading.Tasks.ValueTask> handler)`
- `System.Int64 DroppedPrimaryCount { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsAsciiItem`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsAsciiItem(System.String value)`
- `System.Int32 BodyLength { get; }`
- `System.Int32 Count { get; }`
- `System.String Value { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsBinaryItem`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsBinaryItem(System.Byte[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsBooleanItem`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsBooleanItem(System.Boolean[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity`

- `Dreamine.Secs.Abstractions.Enums.SecsConnectionMode Mode { get; }`
- `Dreamine.Secs.Abstractions.Enums.SecsRole Role { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity <Clone>$()`
- `Dreamine.Secs.Abstractions.Model.SecsSessionId SessionId { get; }`
- `SecsConnectionIdentity(System.String providerKey, System.Guid sessionInstanceId, System.Int64 connectionEpoch, Dreamine.Secs.Abstractions.Model.SecsSessionId sessionId, Dreamine.Secs.Abstractions.Enums.SecsRole role, Dreamine.Secs.Abstractions.Enums.SecsConnectionMode mode)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Model.SecsConnectionIdentity other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Guid SessionInstanceId { get; }`
- `System.Int32 GetHashCode()`
- `System.Int64 ConnectionEpoch { get; }`
- `System.String ProviderKey { get; }`
- `System.String ToString()`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsDialogueDefinition`

- `Dreamine.Secs.Abstractions.Model.SecsDialogueDefinition <Clone>$()`
- `Dreamine.Secs.Abstractions.Model.SecsFunction PrimaryFunction { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsStream Stream { get; }`
- `SecsDialogueDefinition(Dreamine.Secs.Abstractions.Model.SecsStream stream, Dreamine.Secs.Abstractions.Model.SecsFunction primaryFunction, System.Nullable<Dreamine.Secs.Abstractions.Model.SecsFunction> secondaryFunction)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Model.SecsDialogueDefinition other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Boolean ReplyExpected { get; }`
- `System.Int32 GetHashCode()`
- `System.Nullable<Dreamine.Secs.Abstractions.Model.SecsFunction> SecondaryFunction { get; }`
- `System.String ToString()`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsFloat32Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsFloat32Item(System.Single[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsFloat64Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsFloat64Item(System.Double[] values)`
- `System.Int32 BodyLength { get; }`

### `public struct Dreamine.Secs.Abstractions.Model.SecsFunction`

- `SecsFunction(System.Byte value)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Model.SecsFunction other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Boolean IsPrimary { get; }`
- `System.Boolean IsSecondary { get; }`
- `System.Byte Value { get; }`
- `System.Int32 GetHashCode()`
- `System.String ToString()`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsInt16Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsInt16Item(System.Int16[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsInt32Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsInt32Item(System.Int32[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsInt64Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsInt64Item(System.Int64[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsInt8Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsInt8Item(System.SByte[] values)`
- `System.Int32 BodyLength { get; }`

### `public abstract class Dreamine.Secs.Abstractions.Model.SecsItem`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `System.Int32 BodyLength { get; }`
- `System.Int32 Count { get; }`

### `public enum Dreamine.Secs.Abstractions.Model.SecsItemFormat`

- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Ascii = 16`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Binary = 8`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Boolean = 9`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Float32 = 36`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Float64 = 32`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Int16 = 26`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Int32 = 28`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Int64 = 24`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Int8 = 25`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat Jis8 = 17`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat List = 0`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat UInt16 = 42`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat UInt32 = 44`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat UInt64 = 40`
- `const Dreamine.Secs.Abstractions.Model.SecsItemFormat UInt8 = 41`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsJis8Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsJis8Item(System.Byte[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsListItem`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsListItem(Dreamine.Secs.Abstractions.Model.SecsItem[] items)`
- `System.Collections.Generic.IReadOnlyList<Dreamine.Secs.Abstractions.Model.SecsItem> Items { get; }`
- `System.Int32 BodyLength { get; }`
- `System.Int32 Count { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsMessage`

- `Dreamine.Secs.Abstractions.Model.SecsFunction Function { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsItem Item { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsSessionId SessionId { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsStream Stream { get; }`
- `Dreamine.Secs.Abstractions.Model.SecsSystemBytes SystemBytes { get; }`
- `SecsMessage(Dreamine.Secs.Abstractions.Model.SecsSessionId sessionId, Dreamine.Secs.Abstractions.Model.SecsStream stream, Dreamine.Secs.Abstractions.Model.SecsFunction function, System.Boolean replyExpected, Dreamine.Secs.Abstractions.Model.SecsSystemBytes systemBytes, Dreamine.Secs.Abstractions.Model.SecsItem item)`
- `System.Boolean ReplyExpected { get; }`

### `public struct Dreamine.Secs.Abstractions.Model.SecsSessionId`

- `SecsSessionId(System.UInt16 value)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Model.SecsSessionId other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Int32 GetHashCode()`
- `System.String ToString()`
- `System.UInt16 Value { get; }`
- `const System.UInt16 MaximumValue = 32767`

### `public struct Dreamine.Secs.Abstractions.Model.SecsStream`

- `SecsStream(System.Byte value)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Model.SecsStream other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Byte Value { get; }`
- `System.Int32 GetHashCode()`
- `System.String ToString()`

### `public struct Dreamine.Secs.Abstractions.Model.SecsSystemBytes`

- `SecsSystemBytes(System.UInt32 value)`
- `System.Boolean Equals(Dreamine.Secs.Abstractions.Model.SecsSystemBytes other)`
- `System.Boolean Equals(System.Object obj)`
- `System.Int32 GetHashCode()`
- `System.String ToString()`
- `System.UInt32 Value { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsUInt16Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsUInt16Item(System.UInt16[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsUInt32Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsUInt32Item(System.UInt32[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsUInt64Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsUInt64Item(System.UInt64[] values)`
- `System.Int32 BodyLength { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Model.SecsUInt8Item`

- `Dreamine.Secs.Abstractions.Model.SecsItemFormat Format { get; }`
- `SecsUInt8Item(System.Byte[] values)`
- `System.Int32 BodyLength { get; }`

### `public abstract class Dreamine.Secs.Abstractions.Model.SecsValueItem<T>`

- `System.Int32 Count { get; }`
- `System.ReadOnlyMemory<T> Values { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Options.SecsConnectionOptions`

- `Dreamine.Secs.Abstractions.Enums.SecsConnectionMode Mode { get; set; }`
- `Dreamine.Secs.Abstractions.Enums.SecsRole Role { get; set; }`
- `SecsConnectionOptions()`
- `System.String ProviderKey { get; set; }`

### `public sealed class Dreamine.Secs.Abstractions.Options.SecsPrimaryDispatcherOptions`

- `SecsPrimaryDispatcherOptions()`
- `System.Int32 MaximumConcurrency { get; set; }`
- `System.Int32 QueueCapacity { get; set; }`
- `System.Void Validate()`
- `const System.Int32 MaximumHandlerConcurrency = 256`
- `const System.Int32 MaximumQueueCapacity = 65536`

### `public static class Dreamine.Secs.Abstractions.Providers.SecsProviderKeys`

- `const System.String Dreamine = "dreamine"`
- `const System.String EnviaSoft = "enviasoft"`
- `const System.String Linkgenesis = "linkgenesis"`

### `public sealed class Dreamine.Secs.Abstractions.Validation.HsmsStateException`

- `Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState State { get; }`
- `HsmsStateException(Dreamine.Secs.Abstractions.Hsms.HsmsConnectionState state, System.String operation)`
- `System.String Operation { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Validation.HsmsTimerExpiredException`

- `HsmsTimerExpiredException(System.String timerName, System.TimeSpan timeout)`
- `System.String TimerName { get; }`
- `System.TimeSpan Timeout { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Validation.SecsDecodeException`

- `SecsDecodeException(Dreamine.Secs.Abstractions.Validation.SecsValidationCode code, System.String message, System.Nullable<System.Int32> offset)`
- `SecsDecodeException(Dreamine.Secs.Abstractions.Validation.SecsValidationCode code, System.String message, System.Nullable<System.Int32> offset, System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsHeader> hsmsHeader)`
- `System.Nullable<Dreamine.Secs.Abstractions.Hsms.HsmsHeader> HsmsHeader { get; }`

### `public class Dreamine.Secs.Abstractions.Validation.SecsProtocolException`

- `Dreamine.Secs.Abstractions.Validation.SecsValidationCode Code { get; }`
- `SecsProtocolException(Dreamine.Secs.Abstractions.Validation.SecsValidationCode code, System.String message, System.Nullable<System.Int32> offset)`
- `System.Nullable<System.Int32> Offset { get; }`

### `public sealed class Dreamine.Secs.Abstractions.Validation.SecsTransactionTimeoutException`

- `Dreamine.Secs.Abstractions.Model.SecsSystemBytes SystemBytes { get; }`
- `SecsTransactionTimeoutException(Dreamine.Secs.Abstractions.Model.SecsSystemBytes systemBytes, System.TimeSpan timeout)`

### `public enum Dreamine.Secs.Abstractions.Validation.SecsValidationCode`

- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode DepthLimitExceeded = 6`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode InvalidLength = 2`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode InvalidMessage = 8`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode None = 0`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode OutOfRange = 4`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode SizeLimitExceeded = 5`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode TrailingData = 7`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode Truncated = 1`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode UnsupportedFormat = 3`
- `const Dreamine.Secs.Abstractions.Validation.SecsValidationCode UnsupportedProtocolType = 9`

### `public sealed class Dreamine.Secs.Abstractions.Validation.SecsValidationResult`

- `Dreamine.Secs.Abstractions.Validation.SecsValidationCode Code { get; }`
- `Dreamine.Secs.Abstractions.Validation.SecsValidationResult Failure(Dreamine.Secs.Abstractions.Validation.SecsValidationCode code, System.String message, System.Nullable<System.Int32> offset)`
- `Dreamine.Secs.Abstractions.Validation.SecsValidationResult Success { get; }`
- `System.Boolean IsValid { get; }`
- `System.Nullable<System.Int32> Offset { get; }`
- `System.String Message { get; }`
