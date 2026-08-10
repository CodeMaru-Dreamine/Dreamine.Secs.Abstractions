using Dreamine.Secs.Abstractions.Options;
using Dreamine.Secs.Abstractions.Providers;
using Xunit;

namespace Dreamine.Secs.Abstractions.Tests;

public sealed class SecsConnectionOptionsTests
{
    [Fact]
    public void DefaultsToStableDreamineProviderKey()
    {
        var options = new SecsConnectionOptions();

        Assert.Equal("dreamine", SecsProviderKeys.Dreamine);
        Assert.Equal(SecsProviderKeys.Dreamine, options.ProviderKey);
    }
}
