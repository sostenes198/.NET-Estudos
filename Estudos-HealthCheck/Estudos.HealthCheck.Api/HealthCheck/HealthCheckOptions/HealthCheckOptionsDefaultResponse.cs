using System;
using System.Collections.Generic;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Estudos.HealthCheck.Api.HealthCheck.HealthCheckOptions
{
    public class HealthCheckOptionsDefaultResponse : Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        private HealthCheckOptionsDefaultResponse(Func<HealthCheckRegistration, bool> predicate)
        {
            Predicate = predicate ?? (_ => true);
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse;
            ResultStatusCodes = new Dictionary<HealthStatus, int>
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status206PartialContent,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            };
        }

        public static HealthCheckOptionsDefaultResponse Create() => new(null);
        public static HealthCheckOptionsDefaultResponse Create(Func<HealthCheckRegistration, bool> predicate) => new(predicate);
    }
}