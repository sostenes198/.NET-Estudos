using Scheduled.Message.Application.Boundaries.UseCases;

namespace Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

public record TriggeredVollScheduleUseCaseInput(
    DateTime When,
    string JobName,
    string TopicName,
    string Key,
    string? Value,
    string? Headers)
    : IUseCaseInput
{
}