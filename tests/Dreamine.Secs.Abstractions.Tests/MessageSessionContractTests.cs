using System.Reflection;
using Dreamine.Communication.Abstractions.Enums;
using Dreamine.Secs.Abstractions.Diagnostics;
using Dreamine.Secs.Abstractions.Enums;
using Dreamine.Secs.Abstractions.Hsms;
using Dreamine.Secs.Abstractions.Interfaces;
using Dreamine.Secs.Abstractions.Model;
using Dreamine.Secs.Abstractions.Options;
using Xunit;

namespace Dreamine.Secs.Abstractions.Tests;

public sealed class MessageSessionContractTests
{
    [Fact]
    public void ExistingConnectionInterfaceRemainsSourceAndBinaryCompatible()
    {
        var declared = typeof(ISecsConnection)
            .GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(MemberSignature)
            .Where(static value => value.Length != 0)
            .OrderBy(static value => value)
            .ToArray();

        Assert.Equal(new[] { "Property:String ProviderKey" }, declared);
        Assert.Equal(
            new[] { typeof(Dreamine.Communication.Abstractions.Interfaces.IConnectionLifecycle), typeof(IAsyncDisposable) },
            typeof(ISecsConnection).GetInterfaces());
    }

    [Fact]
    public void ExistingProviderInterfaceRemainsSourceAndBinaryCompatible()
    {
        var declared = typeof(ISecsCommunicationProvider)
            .GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Select(MemberSignature)
            .Where(static value => value.Length != 0)
            .OrderBy(static value => value)
            .ToArray();

        Assert.Equal(
            new[]
            {
                "Method:ISecsConnection CreateConnection(SecsConnectionOptions)",
                "Property:String Key"
            },
            declared);
        Assert.Empty(typeof(ISecsCommunicationProvider).GetInterfaces());
    }

    [Fact]
    public void MessageSessionAddsTypedProtocolBoundariesWithoutChangingLegacyInterfaces()
    {
        Assert.Contains(typeof(ISecsConnection), typeof(ISecsMessageSession).GetInterfaces());
        Assert.Contains(typeof(IHsmsWireObservationSource), typeof(ISecsMessageSession).GetInterfaces());
        Assert.Contains(typeof(ISecsCommunicationProvider), typeof(ISecsMessageSessionProvider).GetInterfaces());

        Assert.Equal(typeof(SecsConnectionIdentity), typeof(ISecsMessageSession).GetProperty(nameof(ISecsMessageSession.ConnectionIdentity))!.PropertyType);
        Assert.Equal(typeof(HsmsConnectionState), typeof(ISecsMessageSession).GetProperty(nameof(ISecsMessageSession.HsmsState))!.PropertyType);
        Assert.Equal(typeof(ISecsPrimaryDispatcher), typeof(ISecsMessageSession).GetProperty(nameof(ISecsMessageSession.PrimaryDispatcher))!.PropertyType);
        Assert.Equal(typeof(EventHandler<SecsMessage>), typeof(ISecsMessageSession).GetEvent(nameof(ISecsMessageSession.MessageReceived))!.EventHandlerType);
        Assert.Equal(typeof(EventHandler<SecsDiagnosticEvent>), typeof(ISecsMessageSession).GetEvent(nameof(ISecsMessageSession.DiagnosticReceived))!.EventHandlerType);
        Assert.Equal(typeof(EventHandler<SecsSessionStateChangedEventArgs>), typeof(ISecsMessageSession).GetEvent(nameof(ISecsMessageSession.StateChanged))!.EventHandlerType);
    }

    [Theory]
    [InlineData(0, 1, null)]
    [InlineData(1, 0, null)]
    [InlineData(1, 2, null)]
    [InlineData(1, 255, 0)]
    [InlineData(1, 255, 2)]
    [InlineData(1, 1, 0)]
    [InlineData(1, 1, 4)]
    [InlineData(1, 3, 2)]
    public void DialogueRejectsInvalidStreamOrNonNormalFunctionPair(byte stream, byte primary, int? secondary)
    {
        Assert.ThrowsAny<ArgumentException>(() => new SecsDialogueDefinition(
            new SecsStream(stream),
            new SecsFunction(primary),
            secondary is null ? null : new SecsFunction((byte)secondary.Value)));
    }

    [Fact]
    public void DialogueRepresentsW0WithNoSecondary()
    {
        var dialogue = new SecsDialogueDefinition(new SecsStream(1), new SecsFunction(255));

        Assert.Equal((byte)1, dialogue.Stream.Value);
        Assert.Equal((byte)255, dialogue.PrimaryFunction.Value);
        Assert.Null(dialogue.SecondaryFunction);
        Assert.False(dialogue.ReplyExpected);
    }

    [Theory]
    [InlineData(1, 1, 2)]
    [InlineData(127, 253, 254)]
    public void DialogueRepresentsW1OnlyWithAdjacentNormalSecondary(byte stream, byte primary, byte secondary)
    {
        var dialogue = new SecsDialogueDefinition(
            new SecsStream(stream),
            new SecsFunction(primary),
            new SecsFunction(secondary));

        Assert.True(dialogue.ReplyExpected);
        Assert.Equal(secondary, dialogue.SecondaryFunction!.Value.Value);
    }

    [Fact]
    public void DialogueIsAnImmutableValue()
    {
        Assert.True(typeof(SecsDialogueDefinition).IsSealed);
        Assert.All(
            typeof(SecsDialogueDefinition).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            static property => Assert.False(property.CanWrite));

        var left = new SecsDialogueDefinition(new SecsStream(7), new SecsFunction(5), new SecsFunction(6));
        var right = new SecsDialogueDefinition(new SecsStream(7), new SecsFunction(5), new SecsFunction(6));
        Assert.Equal(left, right);
        Assert.Equal(left.GetHashCode(), right.GetHashCode());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ConnectionIdentityRejectsMissingProviderKey(string? providerKey)
    {
        Assert.ThrowsAny<ArgumentException>(() => CreateIdentity(providerKey!, Guid.NewGuid(), 1, SecsRole.Host, SecsConnectionMode.Active));
    }

    [Fact]
    public void ConnectionIdentityAllowsZeroEpochBeforeFirstSuccessfulTcpConnection()
    {
        var identity = CreateIdentity("provider", Guid.NewGuid(), 0, SecsRole.Host, SecsConnectionMode.Active);

        Assert.Equal(0, identity.ConnectionEpoch);
    }

    [Fact]
    public void ConnectionIdentityRejectsNegativeEpoch()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateIdentity("provider", Guid.NewGuid(), -1, SecsRole.Host, SecsConnectionMode.Active));
    }

    [Theory]
    [InlineData(SecsRole.Unspecified, SecsConnectionMode.Active)]
    [InlineData((SecsRole)999, SecsConnectionMode.Active)]
    [InlineData(SecsRole.Host, SecsConnectionMode.Unspecified)]
    [InlineData(SecsRole.Host, (SecsConnectionMode)999)]
    public void ConnectionIdentityRequiresConcreteRoleAndMode(SecsRole role, SecsConnectionMode mode)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => CreateIdentity("provider", Guid.NewGuid(), 1, role, mode));
    }

    [Fact]
    public void ConnectionIdentityRequiresStableNonEmptySessionInstanceId()
    {
        Assert.Throws<ArgumentException>(() => CreateIdentity("provider", Guid.Empty, 1, SecsRole.Equipment, SecsConnectionMode.Passive));
    }

    [Fact]
    public void ConnectionIdentityIsAnImmutableSnapshot()
    {
        var instanceId = Guid.NewGuid();
        var identity = CreateIdentity("vendor-x", instanceId, 42, SecsRole.Equipment, SecsConnectionMode.Passive);

        Assert.Equal("vendor-x", identity.ProviderKey);
        Assert.Equal(instanceId, identity.SessionInstanceId);
        Assert.Equal(42, identity.ConnectionEpoch);
        Assert.Equal((ushort)7, identity.SessionId.Value);
        Assert.Equal(SecsRole.Equipment, identity.Role);
        Assert.Equal(SecsConnectionMode.Passive, identity.Mode);
        Assert.True(typeof(SecsConnectionIdentity).IsSealed);
        Assert.All(
            typeof(SecsConnectionIdentity).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            static property => Assert.False(property.CanWrite));
    }

    [Fact]
    public void PrimaryDispatcherOptionsUseBoundedConservativeDefaults()
    {
        var options = new SecsPrimaryDispatcherOptions();

        Assert.Equal(128, options.QueueCapacity);
        Assert.Equal(1, options.MaximumConcurrency);
        options.Validate();
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(65_537, 1)]
    [InlineData(1, 0)]
    [InlineData(1, 257)]
    public void PrimaryDispatcherOptionsRejectUnboundedValues(int queueCapacity, int maximumConcurrency)
    {
        var options = new SecsPrimaryDispatcherOptions
        {
            QueueCapacity = queueCapacity,
            MaximumConcurrency = maximumConcurrency
        };

        Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
    }

    [Fact]
    public void HsmsOptionsOwnAndValidatePrimaryDispatcherOptions()
    {
        var defaults = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host
        };
        defaults.Validate();
        Assert.NotNull(defaults.PrimaryDispatcher);

        var invalid = new HsmsSessionOptions
        {
            Mode = SecsConnectionMode.Active,
            Role = SecsRole.Host,
            PrimaryDispatcher = new SecsPrimaryDispatcherOptions { QueueCapacity = 0 }
        };
        Assert.Throws<ArgumentOutOfRangeException>(invalid.Validate);
    }

    [Fact]
    public void DispatcherContractsUseAsyncHandlersAndOneShotReplyShape()
    {
        var handlerType = typeof(Func<ISecsPrimaryContext, CancellationToken, ValueTask>);
        Assert.Equal(handlerType, typeof(ISecsPrimaryDispatcher).GetMethod(nameof(ISecsPrimaryDispatcher.Register))!.GetParameters()[1].ParameterType);
        Assert.Equal(handlerType, typeof(ISecsPrimaryDispatcher).GetMethod(nameof(ISecsPrimaryDispatcher.RegisterFallback))!.GetParameters()[0].ParameterType);

        var reply = typeof(ISecsPrimaryContext).GetMethod(nameof(ISecsPrimaryContext.ReplyAsync))!;
        Assert.Equal(typeof(ValueTask), reply.ReturnType);
        Assert.Equal(new[] { typeof(SecsItem), typeof(CancellationToken) }, reply.GetParameters().Select(static parameter => parameter.ParameterType).ToArray());
        Assert.Equal(typeof(long), typeof(ISecsPrimaryDispatcher).GetProperty(nameof(ISecsPrimaryDispatcher.DroppedPrimaryCount))!.PropertyType);
    }

    [Fact]
    public void SessionStateChangeCarriesTypedPreviousAndCurrentSnapshots()
    {
        var identity = CreateIdentity("provider", Guid.NewGuid(), 9, SecsRole.Equipment, SecsConnectionMode.Passive);
        var change = new SecsSessionStateChangedEventArgs(
            ConnectionState.Connecting,
            ConnectionState.Connected,
            HsmsConnectionState.NotConnected,
            HsmsConnectionState.ConnectedNotSelected,
            identity);

        Assert.Equal(ConnectionState.Connecting, change.PreviousConnectionState);
        Assert.Equal(ConnectionState.Connected, change.CurrentConnectionState);
        Assert.Equal(HsmsConnectionState.NotConnected, change.PreviousHsmsState);
        Assert.Equal(HsmsConnectionState.ConnectedNotSelected, change.CurrentHsmsState);
        Assert.Same(identity, change.ConnectionIdentity);
        Assert.True(typeof(SecsSessionStateChangedEventArgs).IsSealed);
        Assert.True(typeof(EventArgs).IsAssignableFrom(typeof(SecsSessionStateChangedEventArgs)));
        Assert.All(
            typeof(SecsSessionStateChangedEventArgs).GetProperties(BindingFlags.Public | BindingFlags.Instance),
            static property => Assert.False(property.CanWrite));
    }

    [Theory]
    [InlineData((ConnectionState)999, ConnectionState.Connected, HsmsConnectionState.NotConnected, HsmsConnectionState.ConnectedNotSelected)]
    [InlineData(ConnectionState.Connected, (ConnectionState)999, HsmsConnectionState.NotConnected, HsmsConnectionState.ConnectedNotSelected)]
    [InlineData(ConnectionState.Connected, ConnectionState.Disconnected, (HsmsConnectionState)999, HsmsConnectionState.NotConnected)]
    [InlineData(ConnectionState.Connected, ConnectionState.Disconnected, HsmsConnectionState.Selected, (HsmsConnectionState)999)]
    public void SessionStateChangeRejectsUndefinedStates(
        ConnectionState previousConnectionState,
        ConnectionState currentConnectionState,
        HsmsConnectionState previousHsmsState,
        HsmsConnectionState currentHsmsState)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SecsSessionStateChangedEventArgs(
            previousConnectionState,
            currentConnectionState,
            previousHsmsState,
            currentHsmsState,
            CreateIdentity("provider", Guid.NewGuid(), 1, SecsRole.Host, SecsConnectionMode.Active)));
    }

    [Fact]
    public void SessionStateChangeRequiresConnectionIdentity()
    {
        Assert.Throws<ArgumentNullException>(() => new SecsSessionStateChangedEventArgs(
            ConnectionState.Disconnected,
            ConnectionState.Connecting,
            HsmsConnectionState.NotConnected,
            HsmsConnectionState.NotConnected,
            null!));
    }

    private static SecsConnectionIdentity CreateIdentity(
        string providerKey,
        Guid instanceId,
        long epoch,
        SecsRole role,
        SecsConnectionMode mode) =>
        new(providerKey, instanceId, epoch, new SecsSessionId(7), role, mode);

    private static string MemberSignature(MemberInfo member) => member switch
    {
        PropertyInfo property => $"Property:{TypeName(property.PropertyType)} {property.Name}",
        MethodInfo method when !method.IsSpecialName =>
            $"Method:{TypeName(method.ReturnType)} {method.Name}({string.Join(",", method.GetParameters().Select(static parameter => TypeName(parameter.ParameterType)))})",
        _ => string.Empty
    };

    private static string TypeName(Type type) => type.Name;
}
