namespace Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;

public sealed class IdempotentConsumerRepositoryOptions
{
    public SqlServerOptions SqlServerOptions { get; }
    public InMemoryRepositoryOptions InMemoryRepositoryOptions { get; }

    public IdempotentConsumerRepositoryOptions(SqlServerOptions sqlServerOptions, InMemoryRepositoryOptions inMemoryRepositoryOptions)
    {
        SqlServerOptions = sqlServerOptions ?? throw new ArgumentNullException(nameof(sqlServerOptions));
        InMemoryRepositoryOptions = inMemoryRepositoryOptions ?? throw new ArgumentNullException(nameof(inMemoryRepositoryOptions));
    }
}