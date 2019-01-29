using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Enums;

namespace Estudos.IdempotentConsumer.Base;

public interface IMembershipStrategyExtended<in TMessage> : IMembershipStrategy<TMessage>
    where TMessage : IIdempotentConsumerKey
{
    Task RemoveAsync(string instanceId, TMessage message);
    Task SetStateAsync(string instanceId, TMessage message, RepositoryEntryState entryState);
}

public interface IMembershipStrategy<in TMessage> : IMembershipStrategy
    where TMessage : IIdempotentConsumerKey
{
    Task<bool> ExistsAsync(string instanceId, TMessage message);
    Task AddAsync(string instanceId, TMessage message);
}

public interface IMembershipStrategy
{
}