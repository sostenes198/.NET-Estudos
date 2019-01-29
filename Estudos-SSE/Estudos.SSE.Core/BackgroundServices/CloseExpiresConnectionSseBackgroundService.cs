using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services.Contracts;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Core.BackgroundServices
{
    internal sealed class CloseExpiresConnectionSseBackgroundService : IHostedService, IDisposable
    {
        private readonly ISseServiceClientManager _sseServiceClientManager;
        private readonly IClientSseStorage _clientSseStorage;
        private readonly CancellationTokenSource _stoppingCts = new();

        private Task? _executingTask;

        private readonly int _closeConnectionsInSecondsInterval;

        public CloseExpiresConnectionSseBackgroundService(ISseServiceClientManager sseServiceClientManager, IClientSseStorage clientSseStorage, IOptions<CloseExpiresConnectionOptions> options)
        {
            _sseServiceClientManager = sseServiceClientManager;
            _clientSseStorage = clientSseStorage;
            _closeConnectionsInSecondsInterval = options.Value.CloseConnectionsInSecondsInterval;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _executingTask = ExecuteAsync(_stoppingCts.Token);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask is null)
                return;

            try
            {
                _stoppingCts.Cancel();
            }
            // Stryker disable all
            finally
            {
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite, cancellationToken)).ConfigureAwait(false);
            }
            // Stryker restore all
        }

        public void Dispose() => _stoppingCts.Cancel();

        private async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var clients = _sseServiceClientManager.GetClients();

                foreach (var client in clients)
                {
                    if (!await _clientSseStorage.ContainsAsync(client.Id).ConfigureAwait(false))
                        await _sseServiceClientManager.DisconnectClientAsync(client).ConfigureAwait(false);
                }

                await Task.Delay(TimeSpan.FromSeconds(_closeConnectionsInSecondsInterval), CancellationToken.None);
            }
        }
    }
}