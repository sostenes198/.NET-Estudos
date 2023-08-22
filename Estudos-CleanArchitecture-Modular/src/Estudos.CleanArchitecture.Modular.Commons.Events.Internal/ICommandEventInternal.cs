using MediatR;
using Estudos.CleanArchitecture.Modular.Commons.Application.Events;

namespace Estudos.CleanArchitecture.Modular.Commons.Events.Internal;

public interface ICommandEventInternal<out TResult> : IEvent, IRequest<TResult>
    where TResult : class
{
    
}