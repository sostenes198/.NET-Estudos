using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduled.Message.Api.Models;

public record ScheduleVollProcessModel(
    [property: JsonPropertyName("when")] DateTime When,
    [property: JsonPropertyName("jobName")] string JobName,
    [property: JsonPropertyName("topicName")] string TopicName,
    [property: JsonPropertyName("key")] JsonElement Key,
    [property: JsonPropertyName("value")] JsonElement? Value,
    [property: JsonPropertyName("headers")] JsonElement? Headers
);