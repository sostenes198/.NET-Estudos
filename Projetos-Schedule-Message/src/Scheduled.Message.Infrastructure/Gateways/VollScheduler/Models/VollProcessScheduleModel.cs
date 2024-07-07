using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduled.Message.Infrastructure.Gateways.VollScheduler.Models;

public record VollProcessScheduleModel(
    [property: JsonPropertyName("when")] DateTime When,
    [property: JsonPropertyName("jobName")]
    string JobName,
    [property: JsonPropertyName("topicName")]
    string TopicName,
    [property: JsonPropertyName("key")] JsonElement Key,
    [property: JsonPropertyName("value")] JsonElement? Value,
    [property: JsonPropertyName("headers")]
    JsonElement? Headers
);