using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scheduled.Message.Api.Handlers.ProcessTriggeredVollSchedule;
using Scheduled.Message.Api.Presenters.Handlers.TriggeredVollSchedule;
using Scheduled.Message.Api.Presenters.Http.ScheduleVollProcess;
using Scheduled.Message.Application.Boundaries.Scheduler.Handlers;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

namespace Scheduled.Message.Api.Bootstrappers;

[ExcludeFromCodeCoverage]
public static class BootstrapperApi
{
    internal static IServiceCollection InitializeApi(this IServiceCollection services)
    {
        return services
            .InitializeHandlers()
            .InitializePresenters();
    }

    private static IServiceCollection InitializeHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<IProcessTriggeredVollScheduleHandler, ProcessTriggeredVollScheduleHandler>();

        return services;
    }

    private static IServiceCollection InitializePresenters(this IServiceCollection services)
    {
        services.AddPresenter<IScheduleVollProcessUseCaseOutput, ScheduleVollProcessPresenter>();
        services.AddPresenter<ITriggeredVollScheduleOutput, TriggeredVollSchedulePresenter>();

        return services;
    }
}