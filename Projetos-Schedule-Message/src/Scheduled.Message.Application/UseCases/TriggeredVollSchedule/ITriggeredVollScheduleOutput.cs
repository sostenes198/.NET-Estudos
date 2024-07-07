using Scheduled.Message.Application.Boundaries.UseCases.Outputs;

namespace Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

public interface ITriggeredVollScheduleOutput :
    IUseCaseOutput
{
    void Success();
}