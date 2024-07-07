using Scheduled.Message.Application.Boundaries.UseCases.Outputs;

namespace Scheduled.Message.Application.UseCases.ScheduleVollProcess;

public interface IScheduleVollProcessUseCaseOutput :
    IUseCaseOutput
{
    void Success();
}