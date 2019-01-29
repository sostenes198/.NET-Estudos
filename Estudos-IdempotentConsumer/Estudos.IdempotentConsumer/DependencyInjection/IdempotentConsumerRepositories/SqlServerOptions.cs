namespace Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;

public sealed class SqlServerOptions
{
    public string ConnectionString { get; }

    public SqlServerOptions(string connectionString)
    {
        ConnectionString = !string.IsNullOrWhiteSpace(connectionString) ? connectionString : throw new ArgumentNullException(nameof(connectionString));
    }
}