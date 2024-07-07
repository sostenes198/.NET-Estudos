using Scheduled.Message.Domain.ScheduleVollProcess;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class VollProcessScheduleFaker
{
    public static VollProcessSchedule Create(
        DateTime date = new(),
        string jobName = "JOB_NAME",
        string topicName = "TOPIC_NAME",
        string key = @"{""id"": ""ID""}",
        string? value = @"{""id"": ""ID"", ""value"": ""VALUE_1""}",
        string? headers = @"{""h1"": ""H1"", ""h2"": ""H2""}")
    {
        return new VollProcessSchedule(date, jobName, topicName, key, value, headers);
    }
}