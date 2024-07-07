using System.Text.Json;
using Flurl.Http;
using Flurl.Http.Configuration;
using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;
using Scheduled.Message.Domain.ScheduleVollProcess;
using Scheduled.Message.Infrastructure.Gateways.Configurations;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler.Models;

namespace Scheduled.Message.Infrastructure.Gateways.VollScheduler;

public sealed class VollSchedulerGateway(IFlurlClientCache flurlClientCache) : IVollSchedulerGateway
{
    private const string EndpointPublishMessage = "api/v1/publish/message";
    private const string EndpointHealthCheck = "api/v1/healthcheck";

    private readonly IFlurlClient _flurlClient =
        flurlClientCache.Get(GatewayConfigurationsVollConfigurations.ClientName);

    public Task SendScheduledMessageAsync(VollProcessSchedule vollProcessSchedule, CancellationToken token)
    {
        var vollProcessScheduleModel = new VollProcessScheduleModel(
            vollProcessSchedule.When,
            vollProcessSchedule.JobName,
            vollProcessSchedule.TopicName,
            ConvertStringToJsonElement(vollProcessSchedule.Key),
            ConvertStringToJsonElement(vollProcessSchedule.Value),
            ConvertStringToJsonElement(vollProcessSchedule.Headers)
        );

        return _flurlClient
            .Request(EndpointPublishMessage)
            .PostJsonAsync(vollProcessScheduleModel, cancellationToken: token);
    }

    public Task VerifyHealthCheck(CancellationToken token)
    {
        return _flurlClient
            .Request(EndpointHealthCheck)
            .GetAsync(cancellationToken: token);
    }

    private static JsonElement ConvertStringToJsonElement(string? jsonString)
    {
        return JsonSerializer.Deserialize<JsonElement>(jsonString ?? "{}");
    }
}