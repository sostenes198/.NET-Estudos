using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;

namespace Scheduled.Message.Tests.Integration;

public static class GlobalInfrastructure
{
    private static INetwork _network = null!;
    private static IContainer _mongoContainer = null!;

    public static async Task StartAsync()
    {
        _network = new NetworkBuilder()
            .WithName("schedule-message-integration-test")
            .Build();

        _mongoContainer = new ContainerBuilder()
            .WithImage("mongo")
            .WithNetwork(_network)
            .WithNetworkAliases("mongo")
            .WithName("mongo")
            .WithPortBinding("27017", "27017")
            .WithCommand(["--replSet", "rs0", "--bind_ip_all", "--port", "27017"])
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted(@"echo ""try { rs.status() } catch (err) { rs.initiate({_id:'rs0',members:[{_id:0,host:'host.docker.internal:27017'}]}) }"" | mongosh --port 27017 --quiet"))
            .Build();
        
        await _mongoContainer.StartAsync();
    }

    public static async Task StopAsync()
    {
        await _mongoContainer.StopAsync();
        await _mongoContainer.DisposeAsync();

        await _network.DeleteAsync();
    }
}