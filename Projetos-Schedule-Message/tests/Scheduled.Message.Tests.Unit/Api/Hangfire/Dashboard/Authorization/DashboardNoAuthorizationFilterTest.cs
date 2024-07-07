using FluentAssertions;
using Hangfire.Dashboard;
using Hangfire.Mongo;
using Moq;
using Scheduled.Message.Api.Hangfire.Dashboard.Authorization;

namespace Scheduled.Message.Tests.Unit.Api.Hangfire.Dashboard.Authorization;

public class DashboardNoAuthorizationFilterTest
{
    private readonly IDashboardAuthorizationFilter _filter;

    public DashboardNoAuthorizationFilterTest()
    {
        _filter = new DashboardNoAuthorizationFilter();
    }

    [Fact]
    public void Should_Execute_Filter_Successfully()
    {
        // Arrange - Act
        var result = _filter.Authorize(null!);

        // Assert
        result.Should().BeTrue();
    }
}