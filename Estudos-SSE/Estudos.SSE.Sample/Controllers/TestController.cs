using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.SSE.Sample.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ISseServiceSendEvent _sseServiceSendEvent;

    public TestController(ISseServiceSendEvent sseServiceSendEvent)
    {
        _sseServiceSendEvent = sseServiceSendEvent;
    }

    [HttpGet("send/{clientId}")]
    public async Task Receive(string clientId, CancellationToken cancellationToken)
    {
        await _sseServiceSendEvent.SendEventAsync(clientId, "Testando", cancellationToken);
    }
    
    [HttpGet("send-channel/{channel}/{clientId}")]
    public async Task ReceiveChannel(string channel, string clientId, CancellationToken cancellationToken)
    {
        await _sseServiceSendEvent.SendEventAsync(clientId, new SseEvent(channel, "Message"), cancellationToken);
    }
}