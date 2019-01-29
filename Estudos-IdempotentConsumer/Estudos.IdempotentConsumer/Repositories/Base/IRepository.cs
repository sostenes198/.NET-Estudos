namespace Estudos.IdempotentConsumer.Repositories.Base;

public interface IRepository
{
    Task<bool> ContainsAsync(string instanceId, string idempotencyKey);
    
    Task<Entry> GetEntryAsync(string instanceId, string idempotencyKey);

    Task<IEnumerable<Entry>> GetEntriesAsync(string instanceId, int dataFetchThreshold);

    Task AddOrUpdateAsync(Entry data);

    Task RemoveAsync(string instanceId, string idempotencyKey);
}