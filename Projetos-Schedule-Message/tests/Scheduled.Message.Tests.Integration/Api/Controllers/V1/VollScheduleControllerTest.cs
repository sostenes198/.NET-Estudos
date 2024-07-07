using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scheduled.Message.Api.Models;
using Scheduled.Message.Api.Presenters.Base;
using Scheduled.Message.Infrastructure.Gateways.Configurations;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler.Models;

// ReSharper disable VirtualMemberCallInConstructor

namespace Scheduled.Message.Tests.Integration.Api.Controllers.V1;

public class VollScheduleControllerTest : BaseTest
{
    private readonly HttpTest _httpTest;
    private readonly HttpClient _client;

    private const int TimeToWaitSchedule = 10000;


    private readonly string _vollSchedulerGatewayPublishMessageEndpoint;

    public VollScheduleControllerTest()
    {
        _httpTest = new HttpTest();
        _client = Client;

        var baseUrlVollSchedulerGateway = ServiceProvider.GetRequiredService<IOptions<GatewayConfigurations>>().Value
            .VollScheduler
            .BaseUrl;


        _vollSchedulerGatewayPublishMessageEndpoint = $"{baseUrlVollSchedulerGateway}/api/v1/publish/message";
    }

    [Fact]
    public async Task Should_Publish_Message_Using_Default_Scheduler_And_Wait_To_Post_Response_In_Gateway()
    {
        // Arrange
        _httpTest.ForCallsTo(_vollSchedulerGatewayPublishMessageEndpoint)
            .RespondWith("");
        _httpTest.ForCallsTo("*").AllowRealHttp();

        var input = new ScheduleVollProcessModel(
            DateTime.UtcNow.AddSeconds(10),
            "integration-test",
            "Integration_Test",
            JsonSerializer.Deserialize<JsonElement>(@"{""id"": ""ID""}"),
            JsonSerializer.Deserialize<JsonElement>(
                @"{""id"": ""ID"", ""value1"": ""VALUE_1"", ""value2"": ""VALUE_2""}"),
            JsonSerializer.Deserialize<JsonElement>(@"{""h1"": ""HEADER_1"", ""h2"": ""HEADER_2""}"));

        // Act
        var result = await _client.PostAsJsonAsync("api/v1/voll/schedule", input);
        result.EnsureSuccessStatusCode();

        // Assert
        await DefaultDelayToWaitFireForget(TimeToWaitSchedule);

        var httpCallAssertion = await HttpCallAssertion();
        httpCallAssertion.WithVerb(HttpMethod.Post);
    }

    [Fact]
    public async Task Should_Return_Bad_Request_When_Invalid_Input()
    {
        // Arrange
        _httpTest.ForCallsTo(_vollSchedulerGatewayPublishMessageEndpoint)
            .RespondWith("");
        _httpTest.ForCallsTo("*").AllowRealHttp();

        var input = new ScheduleVollProcessModel(
            DateTime.UtcNow.AddSeconds(10),
            string.Empty,
            string.Empty,
            JsonSerializer.Deserialize<JsonElement>("{}"),
            JsonSerializer.Deserialize<JsonElement>("{}"),
            JsonSerializer.Deserialize<JsonElement>("{}"));

        var notificationErrorsResponseExpected = new NotificationErrorsResponse(
            new Dictionary<string, string[]>
            {
                {
                    "JobName", [
                        "'Job Name' must not be empty."
                    ]
                },
                {
                    "TopicName", [
                        "'Topic Name' must not be empty."
                    ]
                }
            }
        );

        // Act
        var result = await _client.PostAsJsonAsync("api/v1/voll/schedule", input);

        var notificationErrorsResponse =
            JsonSerializer.Deserialize<NotificationErrorsResponse>(await result.Content.ReadAsStringAsync())!;

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        notificationErrorsResponse.Should().BeEquivalentTo(notificationErrorsResponseExpected);
    }

    private async Task<HttpCallAssertion> HttpCallAssertion()
    {
        HttpCallAssertion? callAssertion = null;
        const int maxAttempt = 5;
        var count = 0;
        do
        {
            count++;
            try
            {
                callAssertion = _httpTest.ShouldHaveCalled(_vollSchedulerGatewayPublishMessageEndpoint);
                break;
            }
            catch
            {
                if (count == maxAttempt)
                    throw;

                await DefaultDelayToWaitFireForget(TimeToWaitSchedule);
            }
        } while (count < maxAttempt);

        return callAssertion!;
    }

    protected override void DisposeBase()
    {
        _httpTest.Dispose();
    }
}