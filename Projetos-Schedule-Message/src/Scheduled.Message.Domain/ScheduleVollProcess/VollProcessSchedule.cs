namespace Scheduled.Message.Domain.ScheduleVollProcess;

public class VollProcessSchedule(
    DateTime when,
    string jobName,
    string topicName,
    string key,
    string? value,
    string? headers)
{
    public DateTime When { get; } = when;

    public string JobName { get; } = jobName;

    public string TopicName { get; } = topicName;

    public string Key { get; } = key;

    public string? Value { get; } = value;

    public string? Headers { get; } = headers;

    public static VollProcessSchedule Empty => new VollProcessSchedule(
        default,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty);
}