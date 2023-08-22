using System.Text.Json.Serialization;

namespace Estudos.CleanArchitecture.Modular.API.Presenters.Base;

public sealed record NotificationErrorsResponse(
    [property: JsonPropertyName("errors")] IDictionary<string, string[]> Errors
);