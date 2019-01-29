using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Services.Contracts;
using Microsoft.Extensions.Hosting;

namespace Estudos.SSE.Core.BackgroundServices
{
    internal sealed class KeepAliveSseConnectionBackgroundService : IHostedService, IDisposable
    {
        private readonly int _keepAliveConnectionInSecondsInterval = 10;

        private static readonly SseEvent KeepAliveSseEvent = new("KEEPALIVE", "PING");

        private readonly ISseServiceClientManager _sseServiceClientManager;
        private readonly ISseServiceSendEvent _sseServiceSendEvent;
        private readonly CancellationTokenSource _stoppingCts = new();

        private Task? _executingTask;

        public KeepAliveSseConnectionBackgroundService(ISseServiceClientManager sseServiceClientManager, ISseServiceSendEvent sseServiceSendEvent)
        {
            _sseServiceClientManager = sseServiceClientManager;
            _sseServiceSendEvent = sseServiceSendEvent;
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
                var tasks = new List<Task>(10);
                var clients = _sseServiceClientManager.GetClients();

                // Stryker disable all
                tasks.AddRange(
                    clients
                       .Select(client => _sseServiceSendEvent.SendEventAsync(client.Id, KeepAliveSseEvent, stoppingToken))
                       .Where(operationTask => !operationTask.IsCompleted));
                
                await Task.WhenAll(tasks).ConfigureAwait(false);
                await Task.Delay(TimeSpan.FromSeconds(_keepAliveConnectionInSecondsInterval), CancellationToken.None).ConfigureAwait(false);
                // Stryker restore all
            }
        }
    }
}