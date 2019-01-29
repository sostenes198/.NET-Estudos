using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Estudos.HealthCheck.Api.HealthCheck.HealthCheckOptions
{
    public class HealthCheckOptionsStatusResponse : Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
    {
        private HealthCheckOptionsStatusResponse(Func<HealthCheckRegistration, bool> predicate)
        {
            Predicate = predicate ?? (_ => true);
            ResponseWriter = async (context, report) =>
            {
                var result = JsonConvert.SerializeObject(
                    new
                    {
                        statusApplication = report.Status.ToString(),
                        healthChecks = report.Entries.Select(e => new
                        {
                            check = e.Key,
                            ErrorMessage = e.Value.Exception?.Message,
                            description = e.Value.Description,
                            status = Enum.GetName(typeof(HealthStatus), e.Value.Status)
                        })
                    });
                context.Response.ContentType = MediaTypeNames.Application.Json;
                await context.Response.WriteAsync(result);
            };
            ResultStatusCodes = new Dictionary<HealthStatus, int>
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status206PartialContent,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            };
        }

        public static HealthCheckOptionsStatusResponse Create() => new(null);
        public static HealthCheckOptionsStatusResponse Create(Func<HealthCheckRegistration, bool> predicate) => new(predicate);
    }
}