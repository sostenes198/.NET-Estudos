using System.Diagnostics.CodeAnalysis;
using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Consumer.Repository;
using Estudos.IdempotentConsumer.Options;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using Estudos.IdempotentConsumer.Repositories.Slq;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Strategies.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;

[ExcludeFromCodeCoverage]
public static class IdempotentConsumerRepositoryDependencyInjection
{
    public static IServiceCollection AddIdempotentConsumerRepository(this IServiceCollection services, IdempotentConsumerRepositoryOptions options)
    {
        AddIdempotentConsumer(services);
        AddMembershipStrategy(services);
        AddRepositories(services, options);
        return services;
    }

    private static void AddIdempotentConsumer(IServiceCollection services)
    {
        services.TryAddScoped<IIdempotentConsumerRepository, IdempotentConsumerRepository>();
    }

    private static void AddMembershipStrategy(IServiceCollection services)
    {
        services.TryAddScoped<IMembershipStrategy<DefaultIdempotentConsumerKey>, RepositoryMembershipStrategy<DefaultIdempotentConsumerKey>>();
    }

    private static void AddRepositories(IServiceCollection services, IdempotentConsumerRepositoryOptions idempotentConsumerRepositoryOptions)
    {
        services.Configure<SqlRepositoryOptions>(opt => { opt.ConnectionString = idempotentConsumerRepositoryOptions.SqlServerOptions.ConnectionString; });
        services.Configure<InMemoryCircularBufferRepositoryOptions>(opt => { opt.MaxItemsBuffer = idempotentConsumerRepositoryOptions.InMemoryRepositoryOptions.MaxItemsBuffer; });

        services.TryAddSingleton<ISqlService, SqlService>();
        services.TryAddSingleton<IDataContext, DataContext>();
        services.TryAddScoped<IRepository, SqlServerRepository>();
        
        services.TryAddSingleton<IInMemoryCircularBufferRepository, InMemoryCircularBufferRepository>();
    }
}