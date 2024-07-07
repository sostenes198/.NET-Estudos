using Scheduled.Message.Application.UseCases.ScheduleVollProcess;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class ScheduleVollProcessUseCaseInputFaker
{
    public static ScheduleVollProcessUseCaseInput Create(
        DateTime? date = default,
        string jobName = "JOB_NAME",
        string topicName = "TOPIC_NAME",
        string key = "KEY",
        string value = "VALUE",
        string headers = "HEADERS")
    {
        return new ScheduleVollProcessUseCaseInput(
            date ?? DateTime.Now,
            jobName,
            topicName,
            key,
            value,
            headers
        );
    }
}