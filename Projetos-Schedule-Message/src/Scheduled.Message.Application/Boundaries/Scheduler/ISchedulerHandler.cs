namespace Scheduled.Message.Application.Boundaries.Scheduler;

public interface ISchedulerHandler<in TInput>
    where TInput : class
{
    Task ExecuteAsync(TInput? input, CancellationToken token = default);
}