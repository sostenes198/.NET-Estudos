using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Helpers;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services.Contracts;
using Estudos.SSE.Core.Services.Events;
using Microsoft.Extensions.Options;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Services
{
    internal sealed class SseService : ISseService
    {
        private readonly IClientSseStorage _clientSseStorage;
        private readonly IServiceProvider _serviceProvider;

        private readonly ConcurrentDictionary<string, SseClient> _clients = new();

        private event EventHandler<SseClientConnectedArgs>? ClientConnected;

        private event EventHandler<SseClientDisconnectedArgs>? ClientDisconnected;

        public SseService(IClientSseStorage clientSseStorage, IServiceProvider serviceProvider, IOptions<SseServiceOptions> options)
        {
            _clientSseStorage = clientSseStorage;
            _serviceProvider = serviceProvider;

            ClientConnected = (_, args) => options.Value.OnClientConnected?.Invoke(args.ServiceProvider, args.Client);
            ClientDisconnected = (_, args) => options.Value.OnClientDisconnected?.Invoke(args.ServiceProvider, args.Client);
        }

        private void OnClientConnected(SseClient client) => ClientConnected?.Invoke(this, new SseClientConnectedArgs(_serviceProvider, client));

        private void OnClientDisconnected(SseClient client) => ClientDisconnected?.Invoke(this, new SseClientDisconnectedArgs(_serviceProvider, client));

        public async Task<bool> IsClientConnectedAsync(string clientId) => _clients.ContainsKey(clientId) || await _clientSseStorage.ContainsAsync(clientId).ConfigureAwait(false);

        public SseClient? GetClient(in string clientId)
        {
            _clients.TryGetValue(clientId, out var client);

            return client;
        }

        public IReadOnlyCollection<SseClient> GetClients() => _clients.Values.ToArray();

        public async Task ConnectClientAsync(SseClient client)
        {
            await _clientSseStorage.AddAsync(client.Id).ConfigureAwait(false);
            _clients.TryAdd(client.Id, client);
            OnClientConnected(client);
        }

        public async Task DisconnectClientAsync(SseClient client)
        {
            client.Disconnect();
            await _clientSseStorage.RemoveAsync(client.Id).ConfigureAwait(false);
            _clients.TryRemove(client.Id, out _);
            OnClientDisconnected(client);
        }

        public Task SendEventAsync(string clientId, string text) => SendEventAsync(clientId, text, CancellationToken.None);

        public Task SendEventAsync(string clientId, string text, CancellationToken cancellationToken)
        {
            var client = GetClient(clientId);

            return client == null
                ? Task.CompletedTask
                : SendEventAsync(new[] {client}, SseEventHelper.GetEventBytes(text), cancellationToken);
        }

        public Task SendEventAsync(string clientId, SseEvent sseEvent) => SendEventAsync(clientId, sseEvent, CancellationToken.None);

        public Task SendEventAsync(string clientId, SseEvent sseEvent, CancellationToken cancellationToken)
        {
            var client = GetClient(clientId);

            return client == null
                ? Task.CompletedTask
                : SendEventAsync(new[] {client}, SseEventHelper.GetEventBytes(sseEvent), cancellationToken);
        }

        private Task SendEventAsync(IEnumerable<SseClient> clients, SseEventBytes sseEventBytes, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>(10);

            foreach (var client in clients)
            {
                if (!client.IsConnected)
                    continue;

                cancellationToken.ThrowIfCancellationRequested();

                tasks.Add(_clientSseStorage.UpdateAsync(client.Id));
                tasks.Add(client.SendEventAsync(sseEventBytes, cancellationToken));
            }

            // Stryker disable once all
            return tasks.Any() ? Task.WhenAll(tasks) : Task.CompletedTask;
        }
    }
}