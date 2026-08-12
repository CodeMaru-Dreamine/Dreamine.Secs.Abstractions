namespace Dreamine.Secs.Abstractions.Options;

/// <summary>
/// \if KO
/// <para>inbound Primary dispatcher의 bounded queue와 handler 병렬 실행을 설정합니다.</para>
/// <para>accepted Primary는 FIFO로 enqueue됩니다. queue가 가득 차면 dispatcher가 claim한 newest Primary를 drop하고 fallback 또는 legacy event로 넘기지 않습니다. 병렬 실행 수가 1보다 크면 완료 순서는 보장되지 않습니다.</para>
/// \endif
/// \if EN
/// <para>Configures the bounded queue and handler concurrency of the inbound primary dispatcher.</para>
/// <para>Accepted primaries are enqueued in FIFO order. A full queue drops the newest primary already claimed by the dispatcher without falling through to fallback or the legacy event. Completion order is not guaranteed when concurrency is greater than one.</para>
/// \endif
/// </summary>
public sealed class SecsPrimaryDispatcherOptions
{
    /// <summary>\if KO 허용되는 최대 queue capacity입니다. \endif \if EN The maximum permitted queue capacity. \endif</summary>
    public const int MaximumQueueCapacity = 65_536;

    /// <summary>\if KO 허용되는 최대 handler 병렬 실행 수입니다. \endif \if EN The maximum permitted handler concurrency. \endif</summary>
    public const int MaximumHandlerConcurrency = 256;

    /// <summary>\if KO bounded inbound Primary queue capacity를 가져옵니다. \endif \if EN Gets the bounded inbound-primary queue capacity. \endif</summary>
    public int QueueCapacity { get; init; } = 128;

    /// <summary>\if KO 동시에 실행할 최대 Primary handler 수를 가져옵니다. \endif \if EN Gets the maximum number of primary handlers run concurrently. \endif</summary>
    public int MaximumConcurrency { get; init; } = 1;

    /// <summary>\if KO bounded dispatcher 설정을 검증합니다. \endif \if EN Validates the bounded dispatcher settings. \endif</summary>
    public void Validate()
    {
        if (QueueCapacity is < 1 or > MaximumQueueCapacity)
            throw new ArgumentOutOfRangeException(nameof(QueueCapacity));
        if (MaximumConcurrency is < 1 or > MaximumHandlerConcurrency)
            throw new ArgumentOutOfRangeException(nameof(MaximumConcurrency));
    }
}
