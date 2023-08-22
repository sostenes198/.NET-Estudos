using MediatR;

namespace Estudos.CleanArchitecture.Modular.Commons.Events.Internal;

public interface ICommandEventIInternalEventHandler<in TEvent, TResult> : IRequestHandler<TEvent, TResult>
    where TEvent : ICommandEventInternal<TResult>
    where TResult : class
{
}