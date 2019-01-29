using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Estudos.BackgroundServices.BackgroundServices.ServiceWithScope
{
    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            executionCount++;

            _logger.LogInformation(
                "Scoped Processing Service is working. Count: {Count}", executionCount);

            await Task.Delay(100, stoppingToken);
        }
    }
}