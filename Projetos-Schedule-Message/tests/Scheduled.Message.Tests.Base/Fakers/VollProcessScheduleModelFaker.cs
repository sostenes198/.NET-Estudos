using System.Text.Json;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler.Models;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class VollProcessScheduleModelFaker
{
    public static VollProcessScheduleModel Create(DateTime date = new())
    {
        return new VollProcessScheduleModel(date, "JOB_NAME", "TOPIC_NAME",
            JsonSerializer.Deserialize<JsonElement>("{}"),
            JsonSerializer.Deserialize<JsonElement>("{}"),
            JsonSerializer.Deserialize<JsonElement>("{}"));
    }
}