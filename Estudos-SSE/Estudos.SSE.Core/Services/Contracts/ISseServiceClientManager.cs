using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.SSE.Core.Clients;

namespace Estudos.SSE.Core.Services.Contracts
{
    internal interface ISseServiceClientManager
    {
        Task<bool> IsClientConnectedAsync(string clientId);
        SseClient? GetClient(in string clientId);
        IReadOnlyCollection<SseClient> GetClients();
        Task ConnectClientAsync(SseClient client);
        Task DisconnectClientAsync(SseClient client);
    }
}