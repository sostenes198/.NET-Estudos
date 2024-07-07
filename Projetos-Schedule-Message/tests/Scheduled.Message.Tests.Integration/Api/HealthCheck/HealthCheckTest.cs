using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Flurl.Http.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Scheduled.Message.Infrastructure.Gateways.Configurations;

// ReSharper disable UnusedMember.Global
// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

// ReSharper disable VirtualMemberCallInConstructor

namespace Scheduled.Message.Tests.Integration.Api.HealthCheck;

public class HealthCheckTest : BaseTest
{
    public class HealthCheckModelEntries
    {
        [JsonPropertyName("data")] public Dictionary<string, object> Data { get; set; }

        [JsonPropertyName("duration")] public TimeSpan Duration { get; set; }

        [JsonPropertyName("status")] public string Status { get; set; }

        [JsonPropertyName("tags")] public string[] Tags { get; set; }
    }

    public class HealthCheckModel
    {
        [JsonPropertyName("status")] public string Status { get; set; }

        [JsonPropertyName("totalDuration")] public TimeSpan TotalDuration { get; set; }

        [JsonPropertyName("entries")] public Dictionary<string, HealthCheckModelEntries> Entries { get; set; }
    }

    private readonly HttpTest _httpTest;
    private readonly HttpClient _client;

    private readonly string _baseUrl;

    private const string VollSchedulerGatewayHealthCheckEndpoint = "api/v1/healthcheck";

    public HealthCheckTest()
    {
        _httpTest = new HttpTest();
        _client = Client;

        _baseUrl = ServiceProvider.GetRequiredService<IOptions<GatewayConfigurations>>().Value.VollScheduler
            .BaseUrl;;
    }

    [Fact]
    public async Task Should_Validate_HealthCheck_Liveness()
    {
        // Arrang
        _httpTest.ForCallsTo("*").AllowRealHttp();

        var statusExpected = HealthStatus.Healthy.ToString();

        // Act
        var result = await _client.GetAsync("/v1/healthcheck");

        var healthCheckResult = JsonSerializer.Deserialize<HealthCheckModel>(await result.Content.ReadAsStringAsync())!;

        // Assert
        healthCheckResult.Should().NotBeNull();
        healthCheckResult.Status.Should().Be(statusExpected);
    }

    [Fact]
    public async Task Should_Validate_HealthCheck_Readiness()
    {
        // Arrange
        _httpTest.ForCallsTo($"{_baseUrl}/{VollSchedulerGatewayHealthCheckEndpoint}").RespondWith("");
        _httpTest.ForCallsTo("*").AllowRealHttp();

        var statusExpected = HealthStatus.Healthy.ToString();

        // Act
        var result = await _client.GetAsync("/v1/healthcheck/readiness");

        var healthCheckResult = JsonSerializer.Deserialize<HealthCheckModel>(await result.Content.ReadAsStringAsync())!;

        // Assert
        healthCheckResult.Should().NotBeNull();
        healthCheckResult.Status.Should().Be(statusExpected);

        healthCheckResult.Entries["MongoHangfire"].Status.Should().Be(statusExpected);
        healthCheckResult.Entries["MongoHangfire"].Tags.Should().HaveCount(1);
        healthCheckResult.Entries["MongoHangfire"].Tags[0].Should().Be("readiness");

        healthCheckResult.Entries["VollSchedulerGateway"].Status.Should().Be(statusExpected);
        healthCheckResult.Entries["VollSchedulerGateway"].Tags.Should().HaveCount(1);
        healthCheckResult.Entries["VollSchedulerGateway"].Tags[0].Should().Be("readiness");

        _httpTest.ShouldHaveCalled($"{_baseUrl}/{VollSchedulerGatewayHealthCheckEndpoint}")
            .WithVerb(HttpMethod.Get)
            .Times(1);
    }

    protected override void DisposeBase()
    {
        _httpTest.Dispose();
    }
}