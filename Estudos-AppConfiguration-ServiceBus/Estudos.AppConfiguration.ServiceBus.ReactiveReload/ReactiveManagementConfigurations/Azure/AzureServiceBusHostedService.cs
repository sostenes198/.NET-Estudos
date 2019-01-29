using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Azure
{
    internal sealed class AzureServiceBusHostedService : IHostedService, IDisposable
    {
        private readonly IAzureServiceBusTopicSubscription _azureServiceBusTopicSubscription;
        private readonly ILogger<AzureServiceBusHostedService> _logger;

        public AzureServiceBusHostedService(IAzureServiceBusTopicSubscription azureServiceBusTopicSubscription, ILogger<AzureServiceBusHostedService> logger)
        {
            _azureServiceBusTopicSubscription = azureServiceBusTopicSubscription;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await _azureServiceBusTopicSubscription.CreateProcessorAsync(cancellationToken).ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await _azureServiceBusTopicSubscription.CloseProcessorAsync(cancellationToken).ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private async void Dispose(bool disposing)
        {
            if (disposing)
                await _azureServiceBusTopicSubscription.DisposeAsync().ConfigureAwait(false);
        }
    }
}