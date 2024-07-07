using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class TriggeredVollScheduleUseCaseInputFaker
{
    public static TriggeredVollScheduleUseCaseInput Create(
        DateTime? date = default,
        string jobName = "JOB_NAME",
        string topicName = "TOPIC_NAME",
        string key = "KEY",
        string value = "VALUE",
        string headers = "HEADERS")
    {
        return new TriggeredVollScheduleUseCaseInput(
            date ?? DateTime.Now,
            jobName,
            topicName,
            key,
            value,
            headers
        );
    }
}