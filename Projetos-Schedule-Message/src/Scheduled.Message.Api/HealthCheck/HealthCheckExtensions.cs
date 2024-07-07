using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Text.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scheduled.Message.Api.HealthCheck.Customs;
using Scheduled.Message.Api.HealthCheck.HealthCheckOptions;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;

namespace Scheduled.Message.Api.HealthCheck;

[ExcludeFromCodeCoverage]
public static class HealthCheckExtensions
{
    private const string HealthCheckLiveness = "/v1/healthcheck";
    private const string HealthCheckReadiness = HealthCheckLiveness + "/readiness";

    private const string TagLiveness = "liveness";
    private const string TagReadiness = "readiness";

    public static IServiceCollection AddAppHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHealthChecks()
            .AddMongoDb(sp => sp.GetRequiredService<AppMongoDatabaseHangfire>().GetMongoClient(), "MongoHangfire",
                HealthStatus.Unhealthy, new[] { TagReadiness })
            .AddCheck<VollSchedulerGatewayHealthCheck>("VollSchedulerGateway", HealthStatus.Degraded,
                new[] { TagReadiness });

        return services;
    }

    public static void MapHealthChecks(this IEndpointRouteBuilder map)
    {
        map.MapHealthChecks(HealthCheckLiveness, new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
        {
            Predicate = _ => false,
            ResultStatusCodes = new Dictionary<HealthStatus, int>
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            },
            ResponseWriter = async (context, report) =>
            {
                var result = JsonSerializer.Serialize(
                    new
                    {
                        status = report.Status.ToString(),
                    });
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            }
        });

        map.MapHealthChecks(HealthCheckReadiness,
            HealthCheckOptionsDefaultResponse.Create(lnq => lnq.Tags.Contains(TagReadiness)));
    }
}