using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Moq;
using Scheduled.Message.Api.HealthCheck.Customs;
using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Api.HealthCheck.Customs;

public class VollSchedulerGatewayHealthCheckTest : IDisposable
{
    private readonly CancellationToken _token;
    private readonly Mock<IVollSchedulerGateway> _vollSchedulerGatewayMock;
    private readonly Mock<ILogger<VollSchedulerGatewayHealthCheck>> _loggerMock;
    private readonly IHealthCheck _healthCheck;

    public VollSchedulerGatewayHealthCheckTest()
    {
        _token = CancellationToken.None;

        _vollSchedulerGatewayMock = new Mock<IVollSchedulerGateway>(MockBehavior.Strict);

        _loggerMock = new Mock<ILogger<VollSchedulerGatewayHealthCheck>>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(lnq => lnq.GetService(typeof(IVollSchedulerGateway)))
            .Returns(_vollSchedulerGatewayMock.Object);
        serviceProviderMock.Setup(lnq => lnq.GetService(typeof(ILogger<VollSchedulerGatewayHealthCheck>)))
            .Returns(_loggerMock.Object);

        _healthCheck = new VollSchedulerGatewayHealthCheck(serviceProviderMock.Object);
    }

    [Fact]
    public async Task Should_Execute_Health_Check_Successfully()
    {
        // Arrange
        _vollSchedulerGatewayMock.Setup(lnq => lnq.VerifyHealthCheck(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), _token);

        // Assert
        result.Status.Should().Be(HealthStatus.Healthy);
        _vollSchedulerGatewayMock.Verify(lnq => lnq.VerifyHealthCheck(_token), Times.Once);
    }

    [Fact]
    public async Task
        Should_Execute_Health_Check_And_Return_Status_Degraded_When_Failed_To_Verify_Health_Check_Gateway()
    {
        // Arrange
        var exception = new Exception("FAIL");

        _vollSchedulerGatewayMock.Setup(lnq => lnq.VerifyHealthCheck(It.IsAny<CancellationToken>()))
            .Throws(exception);

        // Act
        var result = await _healthCheck.CheckHealthAsync(new HealthCheckContext(), _token);

        // Assert
        result.Status.Should().Be(HealthStatus.Degraded);
        result.Exception.Should().BeEquivalentTo(exception);
        _vollSchedulerGatewayMock.Verify(lnq => lnq.VerifyHealthCheck(_token), Times.Once);
        _loggerMock.VerifyLog(lnq => lnq.LogError(MoqAssert.Assert(exception),
            "Failed {VollSchedulerGatewayHealthCheck}, with message {message}",
            nameof(VollSchedulerGatewayHealthCheck), exception.Message));
    }

    public void Dispose()
    {
        _vollSchedulerGatewayMock.VerifyAll();
        _vollSchedulerGatewayMock.VerifyNoOtherCalls();
    }
}