namespace Estudos.IdempotentConsumer.Base.Keys;

public interface IIdempotentConsumerKey
{
    string IdempotencyKey { get; }
}