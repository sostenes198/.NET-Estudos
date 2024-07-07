using System.Text.Json;
using FluentAssertions;
using Flurl.Http;
using Flurl.Http.Configuration;
using Moq;
using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;
using Scheduled.Message.Domain.ScheduleVollProcess;
using Scheduled.Message.Infrastructure.Gateways.Configurations;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler.Models;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Mocks;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace Scheduled.Message.Tests.Unit.Infrastructure.Gateways.VollScheduler;

public class VollSchedulerGatewayTest : IDisposable
{
    private const string EndpointPublishMessage = "api/v1/publish/message";
    private const string EndpointHealthCheck = "api/v1/healthcheck";

    private readonly CancellationToken _token;

    private readonly Mock<IFlurlClient> _flurlClientMock;
    private readonly Mock<IFlurlClientCache> _flurlClientCacheMock;
    private readonly IVollSchedulerGateway _gateway;

    public VollSchedulerGatewayTest()
    {
        _token = CancellationToken.None;

        _flurlClientMock = new Mock<IFlurlClient>(MockBehavior.Strict);

        _flurlClientCacheMock = new Mock<IFlurlClientCache>(MockBehavior.Strict);
        _flurlClientCacheMock.Setup(lnq => lnq.Get(It.IsAny<string>()))
            .Returns(_flurlClientMock.Object);

        _gateway = new VollSchedulerGateway(_flurlClientCacheMock.Object);
    }

    public record SendScheduleMessageScenario(string Name, VollProcessSchedule vollProcessSchedule);

    public static IEnumerable<object[]> ScenariosSendScheduleMessage()
    {
        yield return
        [
            new SendScheduleMessageScenario("With all values", VollProcessScheduleFaker.Create())
        ];

        yield return
        [
            new SendScheduleMessageScenario("With required values",
                VollProcessScheduleFaker.Create(value: null!, headers: null!))
        ];
    }

    [Theory]
    [MemberData(nameof(ScenariosSendScheduleMessage))]
    public async Task Should_Send_Schedule_Message_Successfully(SendScheduleMessageScenario scenario)
    {
        // Arrange
        var vollProcessScheduleModel = new VollProcessScheduleModel(
            scenario.vollProcessSchedule.When,
            scenario.vollProcessSchedule.JobName,
            scenario.vollProcessSchedule.TopicName,
            JsonSerializer.Deserialize<JsonElement>(scenario.vollProcessSchedule.Key),
            JsonSerializer.Deserialize<JsonElement>(scenario.vollProcessSchedule.Value ?? "{}"),
            JsonSerializer.Deserialize<JsonElement>(scenario.vollProcessSchedule.Headers ?? "{}")
        );

        var flurlRequestMock = FlurlRequestMock.Mock(_flurlClientMock);

        _flurlClientMock.Setup(lnq => lnq.Request(It.IsAny<string>()))
            .Returns(flurlRequestMock.Object);

        // Act
        await _gateway.SendScheduledMessageAsync(scenario.vollProcessSchedule, _token);

        // Assert
        _flurlClientMock.Verify(lnq => lnq.Request(EndpointPublishMessage), Times.Once);
        flurlRequestMock.Verify(lnq => lnq.EnsureClient(), Times.Once);
        flurlRequestMock.VerifyGet(lnq => lnq.Settings, Times.Once);
        flurlRequestMock.Verify(lnq => lnq.SendAsync(HttpMethod.Post,
            It.Is<HttpContent>(t => Match(t, vollProcessScheduleModel)),
            default,
            _token), Times.Once);
    }

    [Fact]
    public async Task Should_Validate_Healthcheck_Successfully()
    {
        // Arrange
        var flurlRequestMock = FlurlRequestMock.Mock(_flurlClientMock);

        _flurlClientMock.Setup(lnq => lnq.Request(It.IsAny<string>()))
            .Returns(flurlRequestMock.Object);

        // Act
        await _gateway.VerifyHealthCheck(_token);

        // Assert
        _flurlClientMock.Verify(lnq => lnq.Request(EndpointHealthCheck), Times.Once);
        flurlRequestMock.Verify(lnq => lnq.SendAsync(HttpMethod.Get,
            null,
            default,
            _token), Times.Once);
    }

    private static bool Match(HttpContent content, VollProcessScheduleModel vollProcessScheduleModel)
    {
        content.ReadAsStringAsync().Result.Should().Be(JsonSerializer.Serialize(vollProcessScheduleModel));
        return true;
    }


    public void Dispose()
    {
        _flurlClientCacheMock.Verify(lnq => lnq.Get(GatewayConfigurationsVollConfigurations.ClientName));
    }
}