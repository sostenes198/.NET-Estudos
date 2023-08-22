using Estudos.CleanArchitecture.Modular.Commons.Application.Events;

namespace Estudos.CleanArchitecture.Modular.Commons.Events;

public interface IDispatcherEvent<in TEvent>
    where TEvent: IEvent
{
    Task DispatchAsync(TEvent @event, CancellationToken cancellationToken = default);
}