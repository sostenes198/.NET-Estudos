using System.Text.Json.Serialization;

namespace Scheduled.Message.Api.Presenters.Base;

public sealed record NotificationErrorsResponse(
    [property: JsonPropertyName("errors")] IDictionary<string, string[]> Errors
);