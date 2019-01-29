using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using Microsoft.Extensions.Configuration;

namespace Estudos.IdempotentConsumer.Tests.Integration.Fixtures;

public class IdempotentConsumerRepositoryOptionsFixture
{
    public static IdempotentConsumerRepositoryOptions Options(IConfiguration configuration) =>
        new(SqlServerOptionsFixture.Options(configuration),
            InMemoryRepositoryOptionsFixture.Options());
}