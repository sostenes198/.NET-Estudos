using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scheduled.Message.Infrastructure.Gateways.Configurations;

namespace Scheduled.Message.Tests.Integration.Infrastructure.Gateways.Configurations;

public class GatewayConfigurationsTest : BaseTest
{
    [Fact]
    public void Should_Validate_Gateway_Configurations()
    {
        // Arrange 
        var expectedResult = new GatewayConfigurations
        {
            VollScheduler = new GatewayConfigurationsVollConfigurations
            {
                BaseUrl = "http://local-voll-scheduler-test.com"
            }
        };

        // Act
        var result = ServiceProvider.GetRequiredService<IOptions<GatewayConfigurations>>().Value;

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}