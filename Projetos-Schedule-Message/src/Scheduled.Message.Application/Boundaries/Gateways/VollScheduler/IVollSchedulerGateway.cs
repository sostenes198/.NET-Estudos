using Scheduled.Message.Domain.ScheduleVollProcess;

namespace Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;

public interface IVollSchedulerGateway
{
    Task SendScheduledMessageAsync(VollProcessSchedule vollProcessSchedule, CancellationToken token = default);

    Task VerifyHealthCheck(CancellationToken token = default);
}