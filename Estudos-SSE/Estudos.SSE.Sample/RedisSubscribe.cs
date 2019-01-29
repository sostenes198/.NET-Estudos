using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Service.Contracts;
using StackExchange.Redis;

namespace Estudos.SSE.Sample;

public class RedisSubscribe : IHostedService
{
    private readonly ISubscriber? _subscriber;
    private readonly ISseServiceSendEvent _sseServiceSendEvent;

    public RedisSubscribe(IConnectionMultiplexer connectionMultiplexer, ISseServiceSendEvent sseServiceSendEvent)
    {
        _subscriber = connectionMultiplexer.GetSubscriber();
        _sseServiceSendEvent = sseServiceSendEvent;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_subscriber is not null)
        {
            await _subscriber?.SubscribeAsync("Test", async (channel, value) =>
            {
                await _sseServiceSendEvent.SendEventAsync(value, new SseEvent("CHANNEL1", "FromSubscribe"), cancellationToken);
            });
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        if (_subscriber is not null)
        {
            return _subscriber.UnsubscribeAsync("Test");
        }

        return Task.CompletedTask;
    }
}