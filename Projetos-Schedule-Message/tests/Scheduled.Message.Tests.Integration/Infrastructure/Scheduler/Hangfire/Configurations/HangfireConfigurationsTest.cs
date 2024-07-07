using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;

namespace Scheduled.Message.Tests.Integration.Infrastructure.Scheduler.Hangfire.Configurations;

public class HangfireConfigurationsTest : BaseTest
{
    [Fact]
    public void Should_Validate_Hangfire_Configurations()
    {
        // Arrange 
        var expectedResult = new HangfireConfigurations
        {
            TtlHangfireDocumentInDays = 60
        };

        // Act
        var result = ServiceProvider.GetRequiredService<IOptions<HangfireConfigurations>>().Value;

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}