using System.Threading;
using System.Threading.Tasks;
using Estudos.SSE.Core.Events;

namespace Estudos.SSE.Core.Services.Contracts
{
    public interface ISseServiceSendEvent
    {
        Task SendEventAsync(string clientId, string text);
        Task SendEventAsync(string clientId, string text, CancellationToken cancellationToken);
        Task SendEventAsync(string clientId, SseEvent sseEvent);
        Task SendEventAsync(string clientId, SseEvent sseEvent, CancellationToken cancellationToken);
    }
}