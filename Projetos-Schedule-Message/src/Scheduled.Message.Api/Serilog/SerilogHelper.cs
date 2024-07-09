using System.Diagnostics.CodeAnalysis;
using Serilog;
using Serilog.Events;

namespace Scheduled.Message.Api.Serilog;

[ExcludeFromCodeCoverage]
public static class SerilogHelper
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

        var headers = request.Headers;
        if (headers.Any())
            diagnosticContext.Set("Headers", headers);
    }

    public static LogEventLevel CustomGetLevel(HttpContext ctx, double _, Exception? ex) =>
        ex != null
            ? LogEventLevel.Error
            : ctx.Response.StatusCode > 499
                ? LogEventLevel.Error
                : IsEndpointToIgnore(ctx) // Not an error, check if it was a health check
                    ? LogEventLevel.Verbose // Was a health check, use Verbose
                    : LogEventLevel.Information;

    private static bool IsEndpointToIgnore(HttpContext ctx)
    {
        var path = ctx.Request.Path.Value;
        if (string.IsNullOrEmpty(path)) return false;

        return path.Contains("swagger", StringComparison.OrdinalIgnoreCase)
               || path.Contains("health", StringComparison.OrdinalIgnoreCase)
               || path.Contains("hangfire", StringComparison.OrdinalIgnoreCase);
    }
}