using MediatR;
using Estudos.CleanArchitecture.Modular.Commons.Application.Events;

namespace Estudos.CleanArchitecture.Modular.Commons.Events.Internal.MediatR;

internal sealed class CommandEventDispatcherMediatR<TEvent, TResult> : ICommandEventDispatcher<TEvent, TResult>
    where TEvent : ICommandEventInternal<TResult>
    where TResult : class
{
    private readonly IMediator _mediator;

    public CommandEventDispatcherMediatR(IMediator mediator)
    {
        _mediator = mediator;
    }
    public Task<TResult> DispatchAsync(TEvent @event, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(@event, cancellationToken);
    }
}