namespace Estudos.Algoritmos.CircularBuffer;

public class Entry
{
    public string InstanceId { get; }

    public string IdempotencyKey { get; }

    public DateTime Timestamp { get; }

    private Entry(string instanceId, string idempotencyKey)
        : this(instanceId, idempotencyKey, DateTime.Now)
    {
    }

    public Entry(string instanceId, string idempotencyKey, DateTime timestamp)
    {
        InstanceId = instanceId;
        IdempotencyKey = idempotencyKey;
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

    public static Entry Empty => new(string.Empty, string.Empty, default);

    public static Predicate<Entry> Predicate(string instanceId, string idempotencyKey)
    {
        var entryToCompare = new Entry(instanceId, idempotencyKey);

        return lnq => lnq != null! && lnq.Equals(entryToCompare);
    }
}