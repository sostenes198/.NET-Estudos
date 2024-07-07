using System.Diagnostics.CodeAnalysis;
using HealthChecks.UI.Client;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Scheduled.Message.Api.HealthCheck.HealthCheckOptions
{
    [ExcludeFromCodeCoverage]
    public class HealthCheckOptionsDefaultResponse : Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        private HealthCheckOptionsDefaultResponse(Func<HealthCheckRegistration, bool>? predicate)
        {
            Predicate = predicate ?? (_ => true);
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse;
            ResultStatusCodes = new Dictionary<HealthStatus, int>
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            };
        }

        public static HealthCheckOptionsDefaultResponse Create() => new(null);
        public static HealthCheckOptionsDefaultResponse Create(Func<HealthCheckRegistration, bool> predicate) => new(predicate);
    }
}