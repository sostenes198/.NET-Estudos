using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Estudos.HealthCheck.Api.HealthCheck
{
    public class ExampleHealthCheck : IHealthCheck
    {
        private static bool _result;
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new())
        {
            _result = !_result;
            if (_result)
            {
                return Task.FromResult(
                    HealthCheckResult.Healthy("A healthy result."));
            }

            return Task.FromResult(
                new HealthCheckResult(context.Registration.FailureStatus, 
                    "An unhealthy result."));
        }
    }
}