using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Application.Boundaries.Scheduler.Handlers;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Domain.ScheduleVollProcess;

namespace Scheduled.Message.Application.UseCases.ScheduleVollProcess;

public sealed class ScheduleVollProcessUseCase(IScheduler scheduler)
    : IUseCase<ScheduleVollProcessUseCaseInput, IScheduleVollProcessUseCaseOutput>
{
    public async Task ExecuteAsync(ScheduleVollProcessUseCaseInput input,
        IScheduleVollProcessUseCaseOutput outputUseCase,
        CancellationToken token = default)
    {
        var vollProcessSchedule = Create(input);

        await scheduler.ScheduleAsync<VollProcessSchedule, IProcessTriggeredVollScheduleHandler>(
            input.JobName,
            vollProcessSchedule,
            vollProcessSchedule.When - DateTime.UtcNow,
            token);

        outputUseCase.Success();
    }

    private VollProcessSchedule Create(ScheduleVollProcessUseCaseInput input)
    {
        return new VollProcessSchedule(
            input.When,
            input.JobName,
            input.TopicName,
            input.Key,
            input.Value,
            input.Headers
        );
    }
}