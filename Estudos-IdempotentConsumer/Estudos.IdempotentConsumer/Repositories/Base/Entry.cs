using Estudos.IdempotentConsumer.Enums;

namespace Estudos.IdempotentConsumer.Repositories.Base;

public class Entry
{
    public string InstanceId { get; }

    public string IdempotencyKey { get; }

    public RepositoryEntryState State { get; }

    public DateTime Timestamp { get; }


    private Entry(string instanceId, string idempotencyKey)
        : this(instanceId, idempotencyKey, RepositoryEntryState.None, DateTime.Now)
    {
    }

    public Entry(string instanceId, string idempotencyKey, RepositoryEntryState state, DateTime timestamp)
    {
        InstanceId = instanceId;
        IdempotencyKey = idempotencyKey;
        State = state;
        Timestamp = timestamp;
    }


    public bool Exist() => !string.IsNullOrWhiteSpace(InstanceId) && !string.IsNullOrWhiteSpace(IdempotencyKey);

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (ReferenceEquals(obj, null)) return false;
        if (GetType() != obj.GetType()) return false;

        var castObj = (Entry) obj;

        return InstanceId == castObj.InstanceId && IdempotencyKey == castObj.IdempotencyKey;
    }

    public override int GetHashCode() => HashCode.Combine(InstanceId, IdempotencyKey);

    public static Entry Empty => new(string.Empty, string.Empty, RepositoryEntryState.None, default);

    public static Predicate<Entry> Predicate(string instanceId, string idempotencyKey)
    {
        var entryToCompare = new Entry(instanceId, idempotencyKey);
        return lnq => lnq != null! && lnq.Equals(entryToCompare);
    }
}