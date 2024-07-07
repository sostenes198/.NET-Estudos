using Scheduled.Message.Application.Boundaries.UseCases;

namespace Scheduled.Message.Application.UseCases.ScheduleVollProcess;

public record ScheduleVollProcessUseCaseInput(
    DateTime When,
    string JobName,
    string TopicName,
    string Key,
    string? Value,
    string? Headers)
    : IUseCaseInput;