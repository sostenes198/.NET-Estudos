namespace Estudos.CleanArchitecture.Modular.Commons.Application.Events;

public interface IEventsDispatcher<in TEvent>
    where TEvent: IEvent
{
    Task DispatchAsync(TEvent @event, CancellationToken cancellationToken = default);
}