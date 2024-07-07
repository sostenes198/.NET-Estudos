namespace Scheduled.Message.Tests.Integration;

public class GlobalFixture : IAsyncLifetime
{
    public Task InitializeAsync()
    {
        return GlobalInfrastructure.StartAsync();
    }

    public Task DisposeAsync()
    {
        return GlobalInfrastructure.StopAsync();
    }
}