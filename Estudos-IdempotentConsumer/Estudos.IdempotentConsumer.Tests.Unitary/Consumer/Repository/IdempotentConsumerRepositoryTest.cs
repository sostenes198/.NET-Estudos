using System;
using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Consumer.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Consumer.Repository;

public class IdempotentConsumerRepositoryTest
{
    private readonly Mock<IMembershipStrategy<DefaultIdempotentConsumerKey>> _mockRepositoryMembershipStrategy;

    public IdempotentConsumerRepositoryTest()
    {
        _mockRepositoryMembershipStrategy = new Mock<IMembershipStrategy<DefaultIdempotentConsumerKey>>();
    }

    [Fact(DisplayName = "Deve instanciar IdempotentConsumerRepository")]
    public void ShouldCreateIdempotentConsumerRepositoryObject()
    {
        // arrange - act 
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new IdempotentConsumerRepository(_mockRepositoryMembershipStrategy.Object);

        // assert
        act.Should().NotThrow();
    }
}