using Estudos.CleanArchitecture.Modular.Commons.Application.Events;

namespace Estudos.CleanArchitecture.Modular.Commons.Events;

internal sealed class EventsDispatcher<TEvent> : IEventsDispatcher<TEvent>
    where TEvent : IEvent
{
    private readonly IEnumerable<IDispatcherEvent<TEvent>> _dispatcherEvents;

    public EventsDispatcher(IEnumerable<IDispatcherEvent<TEvent>> dispatcherEvents)
    {
        _dispatcherEvents = dispatcherEvents;
    }

    public Task DispatchAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        var tasks = new List<Task>(10);
        tasks.AddRange(_dispatcherEvents.Select(dispatcherEvent => dispatcherEvent.DispatchAsync(@event, cancellationToken)));

        return Task.WhenAll(tasks);
    }
}