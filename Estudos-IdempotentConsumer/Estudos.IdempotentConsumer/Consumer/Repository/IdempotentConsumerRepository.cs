using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;

namespace Estudos.IdempotentConsumer.Consumer.Repository;

public class IdempotentConsumerRepository : IdempotentConsumer<DefaultIdempotentConsumerKey>, IIdempotentConsumerRepository
{
    public IdempotentConsumerRepository(IMembershipStrategy<DefaultIdempotentConsumerKey> membershipStrategy) : base(membershipStrategy)
    {
    }
}