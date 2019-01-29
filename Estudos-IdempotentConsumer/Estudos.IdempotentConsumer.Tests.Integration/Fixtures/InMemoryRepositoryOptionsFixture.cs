using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;

namespace Estudos.IdempotentConsumer.Tests.Integration.Fixtures;

public static class InMemoryRepositoryOptionsFixture
{
    public static InMemoryRepositoryOptions Options() => new(50);
}