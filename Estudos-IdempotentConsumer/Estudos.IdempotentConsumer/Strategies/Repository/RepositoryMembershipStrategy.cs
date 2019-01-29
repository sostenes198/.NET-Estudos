using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;

namespace Estudos.IdempotentConsumer.Strategies.Repository;

public class RepositoryMembershipStrategy<TMessage> : IMembershipStrategyExtended<TMessage>
    where TMessage : IIdempotentConsumerKey
{
    private readonly IRepository _repository;

    public RepositoryMembershipStrategy(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExistsAsync(string instanceId, TMessage message)
    {
        if (await _repository.ContainsAsync(instanceId, message.IdempotencyKey))
            return true;

        await InternalAddOrUpdateAsync(instanceId, message, RepositoryEntryState.Reserved);
        return false;
    }

    public Task AddAsync(string instanceId, TMessage message) => InternalAddOrUpdateAsync(instanceId, message, RepositoryEntryState.Committed);

    public Task RemoveAsync(string instanceId, TMessage message) => _repository.RemoveAsync(instanceId, message.IdempotencyKey);
    
    public Task SetStateAsync(string instanceId, TMessage message, RepositoryEntryState entryState) => InternalAddOrUpdateAsync(instanceId, message, entryState);

    private Task InternalAddOrUpdateAsync(string instanceId, TMessage message, RepositoryEntryState entryState)
    {
        var entry = new Entry(instanceId, message.IdempotencyKey, entryState, DateTime.Now);
        return _repository.AddOrUpdateAsync(entry);
    }
}