using Microsoft.Extensions.Diagnostics.HealthChecks;
using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;

namespace Scheduled.Message.Api.HealthCheck.Customs;

public class VollSchedulerGatewayHealthCheck(
    IServiceProvider provider) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = new())
    {
        var gateway = provider.GetRequiredService<IVollSchedulerGateway>();
        var logger = provider.GetRequiredService<ILogger<VollSchedulerGatewayHealthCheck>>();

        try
        {
            await gateway.VerifyHealthCheck(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed {VollSchedulerGatewayHealthCheck}, with message {message}",
                nameof(VollSchedulerGatewayHealthCheck), ex.Message);
            return HealthCheckResult.Degraded(exception: ex);
        }
    }
}