using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Estudos.SSE.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public TestController(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        
    }

    [HttpGet("{clientId}")]
    public void Get(string clientId)
    {
        ISubscriber subscriber = _connectionMultiplexer.GetSubscriber();
        subscriber.Publish("Test", clientId);
    }
}