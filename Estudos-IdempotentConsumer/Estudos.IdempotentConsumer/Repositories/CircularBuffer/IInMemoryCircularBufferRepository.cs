using Estudos.IdempotentConsumer.Repositories.Base;

namespace Estudos.IdempotentConsumer.Repositories.CircularBuffer;

public interface IInMemoryCircularBufferRepository : IRepository
{
    int MaxItemsBuffer { get; }
    void SetEntries(string instanceId, IEnumerable<Entry> entries);
}