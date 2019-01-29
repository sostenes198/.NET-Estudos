using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Contracts
{
    internal interface IAzureServiceBusTopicSubscription : IAsyncDisposable
    {
        Task CreateProcessorAsync(CancellationToken cancellationToken = default);
        Task TryReconnectAsync();
        Task CloseProcessorAsync(CancellationToken cancellationToken = default);
    }
}