using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire;

public interface IHangfireSchedulerRepository<TInput>
    where TInput : class
{
    Task SaveParamsAsync(HangfireParams<TInput> hangfireParams, CancellationToken token = default);

    Task<HangfireParams<TInput>> RecoveryParamsAsync(string id, CancellationToken token = default);
}