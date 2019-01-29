using Estudos.IdempotentConsumer.Options;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Optiosn;

public class InMemoryCircularBufferRepositoryOptionsTest
{
    [Fact(DisplayName = "Deve criar objeto InMemoryCircularBufferRepositoryOptions")]
    public void ShouldCreateInMemoryCircularBufferRepositoryOptionsObject()
    {
        // arrange - act
        var inMemoryCircularBufferRepositoryOptions = new InMemoryCircularBufferRepositoryOptions
        {
            MaxItemsBuffer = 10
        };

        // assert
        inMemoryCircularBufferRepositoryOptions.MaxItemsBuffer.Should().Be(10);
    }
}