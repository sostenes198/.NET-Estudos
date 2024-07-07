using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Scheduled.Message.Api.Models;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;
using JsonElement = System.Text.Json.JsonElement;

namespace Scheduled.Message.Tests.Unit.Api.Models;

public class ScheduleVollProcessModelTest
{
    [Fact]
    public void Should_Validate_Schedule_Voll_Process_Model()
    {
        // Arrange
        var date = new DateTime();

        var expectedResult = new ScheduleVollProcessModel(
            date,
            "JOB_NAME",
            "TOPIC_NAME",
            JsonSerializer.Deserialize<JsonElement>(@"{""id"": ""ID""}"),
            JsonSerializer.Deserialize<JsonElement>(@"{""id"": ""ID"", ""value"": ""VALUE_1""}"),
            JsonSerializer.Deserialize<JsonElement>(@"{""h1"": ""H1"", ""h2"": ""H2""}"));

        // Act
        var result = ScheduleVollProcessModelFaker.Create(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
            opt.Excluding(prop => prop.Key)
                .Excluding(prop => prop.Value)
                .Excluding(prop => prop.Headers)
        );
    }

    [Fact]
    public void Should_Validate_Properties_Schedule_Voll_Process_Model()
    {
        // Arrange - Act - Assert
        typeof(ScheduleVollProcessModel).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "When", Type = typeof(DateTime) },
            new() { Name = "JobName", Type = typeof(string) },
            new() { Name = "TopicName", Type = typeof(string) },
            new() { Name = "Key", Type = typeof(JsonElement) },
            new() { Name = "Value", Type = typeof(JsonElement?) },
            new() { Name = "Headers", Type = typeof(JsonElement?) },
        });

        typeof(ScheduleVollProcessModel).ValidateAttribute(new List<AttributeProperty<JsonPropertyNameAttribute>>
        {
            new()
            {
                Property = nameof(ScheduleVollProcessModel.When),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("when"); }
            },
            new()
            {
                Property = nameof(ScheduleVollProcessModel.JobName),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("jobName"); }
            },
            new()
            {
                Property = nameof(ScheduleVollProcessModel.TopicName),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("topicName"); }
            },
            new()
            {
                Property = nameof(ScheduleVollProcessModel.Key),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("key"); }
            },
            new()
            {
                Property = nameof(ScheduleVollProcessModel.Value),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("value"); }
            },
            new()
            {
                Property = nameof(ScheduleVollProcessModel.Headers),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("headers"); }
            }
        });
    }
}