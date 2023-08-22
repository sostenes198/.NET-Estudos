using MediatR;

namespace Estudos.CleanArchitecture.Modular.Commons.Events.Internal;

public interface IInternalEventHandler<in TEvent> : IRequestHandler<TEvent>
    where TEvent : IEventInternal, IRequest
{
}