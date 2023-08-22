namespace Estudos.CleanArchitecture.Modular.Commons.Application.Events;

public interface IEvent
{
    public Guid Id { get; }

    public DateTime CurredOn { get; }
}