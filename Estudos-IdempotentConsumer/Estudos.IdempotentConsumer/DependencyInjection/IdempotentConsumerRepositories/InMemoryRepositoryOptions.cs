namespace Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;

public sealed class InMemoryRepositoryOptions
{
    public int MaxItemsBuffer { get; }
    
    public InMemoryRepositoryOptions(int maxItemsBuffer = default)
    {
        MaxItemsBuffer = maxItemsBuffer;
    }
}