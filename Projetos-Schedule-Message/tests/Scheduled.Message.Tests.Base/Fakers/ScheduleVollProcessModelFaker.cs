using System.Text.Json;
using Scheduled.Message.Api.Models;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class ScheduleVollProcessModelFaker
{
    public static ScheduleVollProcessModel Create(DateTime? date)
    {
        return new ScheduleVollProcessModel(
            date ?? DateTime.Now,
            "JOB_NAME",
            "TOPIC_NAME",
            JsonSerializer.Deserialize<JsonElement>(@"{""id"": ""ID""}"),
            JsonSerializer.Deserialize<JsonElement>(@"{""id"": ""ID"", ""value"": ""VALUE_1""}"),
            JsonSerializer.Deserialize<JsonElement>(@"{""h1"": ""H1"", ""h2"": ""H2""}")
        );
    }
}