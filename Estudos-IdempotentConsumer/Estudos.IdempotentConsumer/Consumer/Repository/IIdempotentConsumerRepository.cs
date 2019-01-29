using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;

namespace Estudos.IdempotentConsumer.Consumer.Repository;

public interface IIdempotentConsumerRepository : IIdempotentConsumer<DefaultIdempotentConsumerKey>
{
}