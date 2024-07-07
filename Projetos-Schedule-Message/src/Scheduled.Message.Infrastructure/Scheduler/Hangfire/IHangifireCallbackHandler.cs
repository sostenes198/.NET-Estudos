using System.ComponentModel;
using Scheduled.Message.Application.Boundaries.Scheduler;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire;

public interface IHangifireCallbackHandler<THandler, TInput>
    where THandler : ISchedulerHandler<TInput>
    where TInput : class
{
    [DisplayName("{1}")]
    Task ExecuteAsync(string id, string jobName);
}