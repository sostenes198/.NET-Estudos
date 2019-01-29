using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.DependencyInjection.IdempotentConsumerRepositories;

public class InMemoryRepositoryOptionsTest
{
    [Theory(DisplayName = "Deve criar objeto InMemoryRepositoryOptions")]
    [InlineData(default)]
    [InlineData(0)]
    [InlineData(10)]
    public void ShouldCreateInMemoryRepositoryOptions(int? maxItemsBuffer)
    {
        // arange - act
        var result = new InMemoryRepositoryOptions(maxItemsBuffer.GetValueOrDefault());
        
        // assert
        result.MaxItemsBuffer.Should().Be(maxItemsBuffer.GetValueOrDefault());
    }
}