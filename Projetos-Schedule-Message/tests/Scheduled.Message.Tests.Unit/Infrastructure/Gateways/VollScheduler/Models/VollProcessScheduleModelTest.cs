using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler.Models;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Gateways.VollScheduler.Models;

public class VollProcessScheduleModelTest
{
    [Fact]
    public void Should_Validate_Voll_Process_Schedule_Model()
    {
        // Arrange
        var date = new DateTime();

        var expectedResult = new VollProcessScheduleModel(date,
            "JOB_NAME", "TOPIC_NAME",
            JsonSerializer.Deserialize<JsonElement>("{}"),
            JsonSerializer.Deserialize<JsonElement>("{}"),
            JsonSerializer.Deserialize<JsonElement>("{}"));

        // Act
        var result = VollProcessScheduleModelFaker.Create(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
            opt.Excluding(prop => prop.Key)
                .Excluding(prop => prop.Value)
                .Excluding(prop => prop.Headers));
    }

    [Fact]
    public void Should_Validate_Properties_Voll_Process_Schedule_Model()
    {
        // Arrange - Act - Assert
        typeof(VollProcessScheduleModel).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "When", Type = typeof(DateTime) },
            new() { Name = "JobName", Type = typeof(string) },
            new() { Name = "TopicName", Type = typeof(string) },
            new() { Name = "Key", Type = typeof(JsonElement) },
            new() { Name = "Value", Type = typeof(JsonElement?) },
            new() { Name = "Headers", Type = typeof(JsonElement?) }
        });

        typeof(VollProcessScheduleModel).ValidateAttribute(new List<AttributeProperty<JsonPropertyNameAttribute>>
        {
            new()
            {
                Property = nameof(VollProcessScheduleModel.When),
                ValidateAttribute = attr => { attr.Name.Should().Be("when"); }
            },
            new()
            {
                Property = nameof(VollProcessScheduleModel.JobName),
                ValidateAttribute = attr => { attr.Name.Should().Be("jobName"); }
            },
            new()
            {
                Property = nameof(VollProcessScheduleModel.TopicName),
                ValidateAttribute = attr => { attr.Name.Should().Be("topicName"); }
            },
            new()
            {
                Property = nameof(VollProcessScheduleModel.Key),
                ValidateAttribute = attr => { attr.Name.Should().Be("key"); }
            },
            new()
            {
                Property = nameof(VollProcessScheduleModel.Value),
                ValidateAttribute = attr => { attr.Name.Should().Be("value"); }
            },
            new()
            {
                Property = nameof(VollProcessScheduleModel.Headers),
                ValidateAttribute = attr => { attr.Name.Should().Be("headers"); }
            },
        });
    }
}