using MediatR;

namespace Estudos.CleanArchitecture.Modular.Commons.Events.Internal.MediatR;

internal class DispatcherEventMediatR<TEvent> : IDispatcherEvent<TEvent>
    where TEvent : IEventInternal

{
    private readonly IMediator _mediator;

    public DispatcherEventMediatR(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task DispatchAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(@event, cancellationToken);
    }
}