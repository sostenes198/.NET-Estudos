using MediatR;
using Estudos.CleanArchitecture.Modular.Commons.Application.Events;

namespace Estudos.CleanArchitecture.Modular.Commons.Events.Internal;

public interface IEventInternal : IEvent, IRequest
{
}