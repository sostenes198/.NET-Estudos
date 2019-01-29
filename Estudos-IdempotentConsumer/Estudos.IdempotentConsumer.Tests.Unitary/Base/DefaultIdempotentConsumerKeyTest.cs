using Estudos.IdempotentConsumer.Base.Keys;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Base;

public class DefaultIdempotentConsumerKeyTest
{
    [Fact(DisplayName = "Deve criar objeto DefaultIdempotentConsumerKeyRepository")]
    public void ShouldCreateDefaultIdempotentConsumerKeyRepositoryObject()
    {
        // arrange - act
        var result = new DefaultIdempotentConsumerKey("Key");

        // assert
        result.IdempotencyKey.Should().BeEquivalentTo("Key");
    }
}