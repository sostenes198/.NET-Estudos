using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

namespace Scheduled.Message.Api.Bootstrappers;

[ExcludeFromCodeCoverage]
public static class BootstrapperApplication
{
    internal static IServiceCollection InitializeApplication(this IServiceCollection services)
    {
        return services
            .InitializeUseCases();
    }

    private static IServiceCollection InitializeUseCases(this IServiceCollection services)
    {
        return services
            .InitializeUseCaseScheduleVollProcess()
            .InitializeUseCaseTriggeredVollSchedule();
    }

    private static IServiceCollection InitializeUseCaseScheduleVollProcess(this IServiceCollection services)
    {
        services
            .TryAddScoped<IUseCase<ScheduleVollProcessUseCaseInput, IScheduleVollProcessUseCaseOutput>,
                ScheduleVollProcessUseCase>();
        services
            .TryAddSingleton<IValidator<ScheduleVollProcessUseCaseInput>, ScheduleVollProcessUseCaseInputValidator>();

        return services;
    }

    private static IServiceCollection InitializeUseCaseTriggeredVollSchedule(this IServiceCollection services)
    {
        services
            .TryAddScoped<IUseCase<TriggeredVollScheduleUseCaseInput, ITriggeredVollScheduleOutput>,
                TriggeredVollScheduleUseCase>();
        services
            .TryAddSingleton<IValidator<TriggeredVollScheduleUseCaseInput>, TriggeredVollScheduleUseCaseInputValidator>();

        return services;
    }
}