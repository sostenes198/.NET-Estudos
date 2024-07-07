using Scheduled.Message.Domain.ScheduleVollProcess;

namespace Scheduled.Message.Application.Boundaries.Scheduler.Handlers;

public interface IProcessTriggeredVollScheduleHandler : ISchedulerHandler<VollProcessSchedule>;