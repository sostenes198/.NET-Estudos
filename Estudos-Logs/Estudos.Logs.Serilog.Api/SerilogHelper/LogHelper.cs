using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Estudos.Logs.Serilog.Api.SerilogHelper
{
    public static class LogHelper
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            var request = httpContext.Request;

            diagnosticContext.Set("Host", request.Host);
            diagnosticContext.Set("Protocol", request.Protocol);
            diagnosticContext.Set("Scheme", request.Scheme);

            if (request.QueryString.HasValue)
            {
                diagnosticContext.Set("QueryString", request.QueryString.Value);
            }

            diagnosticContext.Set("ContentType", httpContext.Response.ContentType);

            var endpoint = httpContext.GetEndpoint();
            if (endpoint is not null) // endpoint != null
            {
                diagnosticContext.Set("EndpointName", endpoint.DisplayName);
            }

            var headers = request.Headers;
            if (headers.Any())
                diagnosticContext.Set("Headers", headers);
        }

        public static LogEventLevel CustomGetLevel(HttpContext ctx, double _, Exception ex) =>
            ex != null
                ? LogEventLevel.Error
                : ctx.Response.StatusCode > 499
                    ? LogEventLevel.Error
                    : IsHealthCheckEndpoint(ctx) // Not an error, check if it was a health check
                        ? LogEventLevel.Verbose // Was a health check, use Verbose
                        : LogEventLevel.Information;

        private static bool IsHealthCheckEndpoint(HttpContext ctx)
        {
            var endpoint = ctx.GetEndpoint();
            if (endpoint?.DisplayName is not null) // same as !(endpoint is null)
            {
                return string.Equals(
                           endpoint.DisplayName,
                           "Health checks",
                           StringComparison.Ordinal)
                       || endpoint.DisplayName.Contains("/health");
            }

            // No endpoint, so not a health check endpoint
            return false;
        }
    }
}