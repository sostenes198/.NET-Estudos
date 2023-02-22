namespace DocumentManager.Core.Services;

public interface ISystemClock
{
    DateTime UtcNow { get; }
}