using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Repositories.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.IdempotentConsumer.DependencyInjection;

public static class IdempotentConsumerDependencyInjection
{
    public static IServiceCollection AddIdempotentConsumer<TIdempotentConsumerImplementation>(this IServiceCollection services, Action<IServiceCollection>? addExtraConfigs = null)
        where TIdempotentConsumerImplementation : IIdempotentConsumer
    {
        var idempotentConsumerType = typeof(TIdempotentConsumerImplementation);
        var idempotentConsumerBaseType = typeof(IIdempotentConsumer<>);
        var idempotentConsumerInterfaceType = GetGenericInterface(idempotentConsumerType, idempotentConsumerBaseType.Name);

        services.TryAddScoped(idempotentConsumerInterfaceType, idempotentConsumerType);

        InvokeExtraConfigs(services, addExtraConfigs);

        return services;
    }

    public static IServiceCollection AddIdempotentConsumerMembershipStrategy<TMembershipStrategy>(this IServiceCollection services, Action<IServiceCollection>? addExtraConfigs = null)
        where TMembershipStrategy : IMembershipStrategy
    {
        var membershipStrategyType = typeof(TMembershipStrategy);
        var membershipStrategyBaseType = typeof(IMembershipStrategy<>);
        var membershipStrategyInterfaceType = GetGenericInterface(membershipStrategyType, membershipStrategyBaseType.Name);

        services.TryAddScoped(membershipStrategyInterfaceType, membershipStrategyType);
        
        InvokeExtraConfigs(services, addExtraConfigs);

        return services;
    }

    public static IServiceCollection AddIdempotentConsumerRepository<TRepository>(this IServiceCollection services, ServiceLifetime serviceLifetime, Action<IServiceCollection>? addExtraConfigs = null)
        where TRepository: IRepository
    {
        var repositoryType = typeof(TRepository);
        var repositoryBaseType = typeof(IRepository);
        var repositoryInterfaceType = GetGenericInterface(repositoryType, repositoryBaseType.Name);

        services.TryAdd(ServiceDescriptor.Describe(repositoryInterfaceType, repositoryType, serviceLifetime));
        
        InvokeExtraConfigs(services, addExtraConfigs);

        return services;
    }

    private static void InvokeExtraConfigs(IServiceCollection services, Action<IServiceCollection>? addExtraConfigs)
    {
        addExtraConfigs?.Invoke(services);
    }

    private static Type GetGenericInterface(Type type, string interfaceName)
    {
        return type.GetInterface(interfaceName) ?? throw new ArgumentException($"{type.Name} should inherit from {interfaceName}");
    }
}