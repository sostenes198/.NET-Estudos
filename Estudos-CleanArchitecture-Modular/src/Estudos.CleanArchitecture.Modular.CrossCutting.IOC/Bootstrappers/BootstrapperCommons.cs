using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Estudos.CleanArchitecture.Modular.Commons.Application.Events;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Estudos.CleanArchitecture.Modular.Commons.Events;
using Estudos.CleanArchitecture.Modular.Commons.Events.Internal.MediatR;
using Estudos.CleanArchitecture.Modular.Infrastructure.UseCases;
using Estudos.CleanArchitecture.Modular.Infrastructure.UseCases.Validators;

namespace Estudos.CleanArchitecture.Modular.CrossCutting.IOC.Bootstrappers;

internal static class BootstrapperCommons
{
    internal static IServiceCollection InitializeCommonsModules(this IServiceCollection services)
    {
        return services
           .AddUseCase()
           .AddEvents()
           .AddEventsInternalCommunication();
    }

    private static IServiceCollection AddUseCase(this IServiceCollection services)
    {
        services.TryAddSingleton(typeof(IUseCaseInputValidatorService<>), typeof(UseCaseInputValidatorService<>));
        services.TryAddScoped<IUseCaseManager, UseCaseManager>();

        return services;
    }

    private static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(IEventsDispatcher<>), typeof(EventsDispatcher<>));

        return services;
    }

    private static IServiceCollection AddEventsInternalCommunication(this IServiceCollection services)
    {
        services.TryAddScoped(typeof(ICommandEventDispatcher<,>), typeof(CommandEventDispatcherMediatR<,>));
        services.TryAddScoped(typeof(IDispatcherEvent<>), typeof(DispatcherEventMediatR<>));

        return services;
    }
}