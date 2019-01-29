using System.Collections.Concurrent;
using Estudos.IdempotentConsumer.Options;
using Estudos.IdempotentConsumer.Repositories.Base;
using Microsoft.Extensions.Options;

namespace Estudos.IdempotentConsumer.Repositories.CircularBuffer;

public sealed class InMemoryCircularBufferRepository : IInMemoryCircularBufferRepository
{
    private const int MaxBufferSizeDefault = 100;
    private readonly ConcurrentDictionary<string, EntryCircularBufferDt> _buffer;

    public int MaxItemsBuffer { get; }

    public InMemoryCircularBufferRepository(IOptions<InMemoryCircularBufferRepositoryOptions> options)
    {
        _buffer = new ConcurrentDictionary<string, EntryCircularBufferDt>();
        MaxItemsBuffer = options.Value != null && options.Value.MaxItemsBuffer != 0 ?  options.Value.MaxItemsBuffer :  MaxBufferSizeDefault;
    }

    public Task<Entry> GetEntryAsync(string instanceId, string idempotencyKey) =>
        Task.FromResult(_buffer.TryGetValue(instanceId, out var circularBufferDt) ? circularBufferDt.GetEntry(instanceId, idempotencyKey) : Entry.Empty);

    public Task<IEnumerable<Entry>> GetEntriesAsync(string instanceId, int dataFetchThreshold) =>
        Task.FromResult(_buffer.TryGetValue(instanceId, out var circularBufferDt) ? circularBufferDt.GetEntries().Take(dataFetchThreshold) : Array.Empty<Entry>());

    public Task<bool> ContainsAsync(string instanceId, string idempotencyKey) => Task.FromResult(_buffer.TryGetValue(instanceId, out var circularBufferDt) && circularBufferDt.Contains(instanceId, idempotencyKey));

    public Task AddOrUpdateAsync(Entry data)
    {
        var instanceId = data.InstanceId;

        if (!_buffer.TryGetValue(instanceId, out var circularBufferDt))
        {
            circularBufferDt = new EntryCircularBufferDt(MaxItemsBuffer);
            _buffer.TryAdd(instanceId, circularBufferDt);
        }

        circularBufferDt.AddOrUpdate(data);

        return Task.CompletedTask;
    }

    public Task RemoveAsync(string instanceId, string idempotencyKey)
    {
        if (!_buffer.TryGetValue(instanceId, out var circularBufferDt))
            return Task.CompletedTask;

        circularBufferDt.Remove(instanceId, idempotencyKey);

        if (circularBufferDt.IsEmpty)
            _buffer.TryRemove(instanceId, out _);

        return Task.CompletedTask;
    }

    public void SetEntries(string instanceId, IEnumerable<Entry> entries)
    {
        if (_buffer.TryGetValue(instanceId, out var circularBufferDt))
            return;

        circularBufferDt = new EntryCircularBufferDt(MaxItemsBuffer, entries.Take(MaxItemsBuffer).ToArray());
        _buffer.TryAdd(instanceId, circularBufferDt);
    }
}