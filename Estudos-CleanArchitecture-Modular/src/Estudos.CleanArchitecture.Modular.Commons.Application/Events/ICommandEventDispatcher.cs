namespace Estudos.CleanArchitecture.Modular.Commons.Application.Events;

public interface ICommandEventDispatcher<in TEvent, TResult>
    where TEvent : IEvent
    where TResult : class
{
    Task<TResult> DispatchAsync(TEvent @event, CancellationToken cancellationToken = default);
}