namespace Estudos.IdempotentConsumer.Base.Keys;

public struct DefaultIdempotentConsumerKey : IIdempotentConsumerKey
{
    public string IdempotencyKey { get; }

    public DefaultIdempotentConsumerKey(string idempotencyKey)
    {
        IdempotencyKey = idempotencyKey;
    }
}