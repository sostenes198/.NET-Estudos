using System.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Application.Exentions;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire;

public sealed class HangifireCallbackHandler<THandler, TInput>(
    IServiceProvider provider,
    IHangfireSchedulerRepository<TInput> schedulerRepository,
    ILogger<HangifireCallbackHandler<THandler, TInput>> logger) : IHangifireCallbackHandler<THandler, TInput>
    where THandler : ISchedulerHandler<TInput>
    where TInput : class
{
    
    public async Task ExecuteAsync(string id, string jobName)
    {
        using (logger.BeginNamedScope("schedule",
                   ("id", id),
                   ("JobName", jobName)))
        {
            var handler = provider.GetRequiredService<THandler>();
            logger.LogInformation("{id} Job execution started", id);

            var hangfireParams = await schedulerRepository.RecoveryParamsAsync(id, CancellationToken.None);

            await handler.ExecuteAsync(hangfireParams.Input);

            logger.LogInformation("{id} Job execution successfully completed", id);
        }
    }
}