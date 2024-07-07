using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Domain.ScheduleVollProcess;

namespace Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

public class TriggeredVollScheduleUseCase(IVollSchedulerGateway gateway)
    : IUseCase<TriggeredVollScheduleUseCaseInput, ITriggeredVollScheduleOutput>
{
    public async Task ExecuteAsync(TriggeredVollScheduleUseCaseInput input, ITriggeredVollScheduleOutput outputUseCase,
        CancellationToken token = default)
    {
        var vollProcessSchedule = Create(input);

        await gateway.SendScheduledMessageAsync(vollProcessSchedule, token);

        outputUseCase.Success();
    }

    private VollProcessSchedule Create(TriggeredVollScheduleUseCaseInput input)
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