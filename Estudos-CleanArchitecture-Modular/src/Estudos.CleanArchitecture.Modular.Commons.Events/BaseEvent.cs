using Estudos.CleanArchitecture.Modular.Commons.Application.Events;

namespace Estudos.CleanArchitecture.Modular.Commons.Events;

public abstract class BaseEvent: IEvent
{
    public Guid Id { get; }

    public DateTime CurredOn { get; }
    
    protected BaseEvent()
    {
        Id = Guid.NewGuid();
        CurredOn = DateTime.Now;
    }
}