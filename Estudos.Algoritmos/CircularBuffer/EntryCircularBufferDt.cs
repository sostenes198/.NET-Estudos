using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Estudos.Algoritmos.CircularBuffer;

public sealed record EntryCircularBufferDt : IEnumerable<Entry>
{
    private const int NotFindIndexElement = -1;

    private static readonly object LockObject = new();

    private readonly int _capacity;
    private readonly bool _emptyCapacity;
    private readonly Entry?[] _buffer;

    private int _index;

    public EntryCircularBufferDt(int capacity)
        : this(capacity, Array.Empty<Entry>())
    {
    }

    public EntryCircularBufferDt(int capacity, Entry[] items)
    {
        if (capacity < 0)
            throw new ArgumentException("Circular buffer cannot have negative capacity.", nameof(capacity));

        if (items == null)
            throw new ArgumentNullException(nameof(items));

        if (items.Length > capacity)
            throw new ArgumentException("Too many items to fit circular buffer.", nameof(items));

        _capacity = capacity;
        _emptyCapacity = _capacity == 0;
        _buffer = new Entry[_capacity];

        Array.Copy(items, _buffer, items.Length);

        _index = GetEntries().Count();
    }

    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Entry> GetEnumerator() => ((IEnumerable<Entry>) _buffer).GetEnumerator();

    public bool IsEmpty => _emptyCapacity || Array.TrueForAll(_buffer, lnq => lnq == null || lnq.Equals(Entry.Empty));

    public int Lenght => _buffer.Length;

    public Entry GetEntry(string instanceId, string idempotencyKey)
    {
        if (_emptyCapacity)
            return Entry.Empty;

        var findIndex = FindIndex(instanceId, idempotencyKey);
        return IsFindIndex(findIndex) ? _buffer[findIndex]! : Entry.Empty;
    }

    public IEnumerable<Entry> GetEntries() => _buffer.Where(lnq => lnq != null)!;

    public bool Contains(string instanceId, string idempotencyKey)
    {
        if (_emptyCapacity)
            return false;

        var findIndex = FindIndex(instanceId, idempotencyKey);
        return IsFindIndex(findIndex);
    }

    public void AddOrUpdate(Entry item)
    {
        if (_emptyCapacity)
            return;

        lock (LockObject)
        {
            var findIndex = FindIndex(item.InstanceId, item.IdempotencyKey);
            if (IsFindIndex(findIndex))
                UpdateItem(findIndex, item);
            else
                AddItem(item);
        }
    }

    public void Remove(string instanceId, string idempotencyKey)
    {
        if (_emptyCapacity)
            return;

        lock (LockObject)
        {
            var findIndex = FindIndex(instanceId, idempotencyKey);
            if (IsFindIndex(findIndex))
                _buffer[findIndex] = null;
        }
    }

    private static bool IsFindIndex(int index) => index != NotFindIndexElement;

    private int FindIndex(string instanceId, string idempotencyKey) => Array.FindIndex(_buffer, Entry.Predicate(instanceId, idempotencyKey)!);

    private void UpdateItem(int index, Entry item) => _buffer[index] = item;

    private void AddItem(Entry item)
    {
        if (_index >= _capacity)
            _index = 0;

        _buffer[_index] = item;
        _index++;
    }
}