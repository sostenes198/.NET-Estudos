using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using Microsoft.Extensions.Configuration;

namespace Estudos.IdempotentConsumer.Tests.Integration.Fixtures;

public static class SqlServerOptionsFixture
{
    
    public static SqlServerOptions Options(IConfiguration configuration) => new(configuration[AppSettingsConstants.SqlServerConnectionString]);
}