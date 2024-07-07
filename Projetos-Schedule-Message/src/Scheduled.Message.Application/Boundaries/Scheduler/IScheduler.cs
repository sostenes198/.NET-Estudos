namespace Scheduled.Message.Application.Boundaries.Scheduler;

public interface IScheduler
{
    Task ScheduleAsync<TInput, THandler>(string jobName, TInput input, TimeSpan delayToWait, CancellationToken token = default)
        where TInput : class
        where THandler : ISchedulerHandler<TInput>;
}