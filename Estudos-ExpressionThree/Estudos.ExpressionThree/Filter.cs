namespace Estudos.ExpressionThree;

public interface IConsumerFilter<TEnvelope> : IConsumerFilter
    where TEnvelope : IEnvelopeBase
{
    /// <summary>
    /// Method implemented by filters. To call the next filter component in the filters pipeline.
    /// </summary>
    /// <param name="consumerFilterContext">The <see cref="ConsumerFilterContext{TEnvelope}"/> instance.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> instance.</param>
    /// <returns>the <see cref="Task"/> instance.</returns>
    Task MoveNextAsync(ConsumerFilterContext<TEnvelope> consumerFilterContext, CancellationToken cancellationToken, Type type = null);
}

public interface IConsumerFilter
{
}

public class ConsumerFilterContext<TEnvelope>
    where TEnvelope : IEnvelopeBase
{
    public TEnvelope Envelope { get; }

    public ConsumerContext Context { get; }

    public ConsumerFilterContext(TEnvelope envelope, ConsumerContext context)
    {
        Envelope = envelope;
        Context = context;
    }
}

public class ConsumerContext
{
    public dynamic Metadata { get; set; }

    public bool IsRetrying { get; set; }

    public int RetryAttempt { get; set; }

    public bool LastAttempt { get; set; }

    protected bool Equals(ConsumerContext other) => Equals(Metadata, other.Metadata) && IsRetrying == other.IsRetrying && RetryAttempt == other.RetryAttempt && LastAttempt == other.LastAttempt;

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (obj.GetType() != this.GetType())
        {
            return false;
        }

        return Equals((ConsumerContext) obj);
    }

    public override int GetHashCode() => HashCode.Combine(Metadata, IsRetrying, RetryAttempt, LastAttempt);
}

public interface IEnvelope : IEnvelopeBase
{
    IDictionary<string, byte[]> Headers { get; set; }
}

public interface IEnvelopeBase
{
    string GroupId { get; }
}

internal sealed class ConsumerFilterInitialize<TEnvelope>
    where TEnvelope : IEnvelopeBase, new()
{
    private readonly IConsumerFilter<TEnvelope> _consumerFilter;

    public ConsumerFilterInitialize(IConsumerFilter<TEnvelope> consumerFilter)
    {
        _consumerFilter = consumerFilter;
    }

    public Task StartExecutionAsync(TEnvelope request, ConsumerContext context, CancellationToken cancellationToken, Type type = null)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var consumerFilterContext = new ConsumerFilterContext<TEnvelope>(request, context);

        return _consumerFilter.MoveNextAsync(consumerFilterContext, cancellationToken, type);
    }
}

internal sealed class LastConsumerFilter<TEnvelope> : IConsumerFilter<TEnvelope>
    where TEnvelope : IEnvelope, new()
{
    public Task MoveNextAsync(ConsumerFilterContext<TEnvelope> consumerFilterContext, CancellationToken cancellationToken, Type type = null)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return Task.CompletedTask;
    }
}

public interface IConsumerBase<in TInput> : IConsumerBase
    where TInput : notnull, IEnvelopeBase
{

    public bool ShouldProcess(Dictionary<string, string> headers);

    public Task ProcessorAsync(TInput request, ConsumerContext context, CancellationToken cancellationToken, Type type = null);

    public Task StartExecutionAsync(TInput request, ConsumerContext context, CancellationToken cancellationToken, Type type = null);

    public Task OnResume(ConsumerContext context, CancellationToken cancellationToken);
    /// <summary>
    /// Default 500ms
    /// </summary>
    public int PauseDelayInMs { get; }
}

public interface IConsumerBaseMultiple
{

}

public interface IConsumerBase
{

}