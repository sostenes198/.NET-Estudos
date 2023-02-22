namespace DocumentManager.Core.Services;

public class SystemClock : ISystemClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}