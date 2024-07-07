using FluentAssertions;
using Scheduled.Message.Domain.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Domain.ScheduleVollProcess;

public class VollProcessScheduleTest
{
    [Fact]
    public void Should_Validate_Voll_Process_Schedule_Test()
    {
        // Arrange
        var date = new DateTime();

        var expectedResult = new VollProcessSchedule(
            date,
            "JOB_NAME",
            "TOPIC_NAME",
            @"{""id"": ""ID""}",
            @"{""id"": ""ID"", ""value"": ""VALUE_1""}",
            @"{""h1"": ""H1"", ""h2"": ""H2""}");

        // Act
        var result = VollProcessScheduleFaker.Create(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Voll_Process_Schedule_Empty()
    {
        // Arrange
        var expectedResult = new VollProcessSchedule(
            default,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty);

        // Act
        var result = VollProcessSchedule.Empty;

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Voll_Process_Schedule()
    {
        // Arrange - Act - Assert
        typeof(VollProcessSchedule).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "When", Type = typeof(DateTime) },
            new() { Name = "JobName", Type = typeof(string) },
            new() { Name = "TopicName", Type = typeof(string) },
            new() { Name = "Key", Type = typeof(string) },
            new() { Name = "Value", Type = typeof(string) },
            new() { Name = "Headers", Type = typeof(string) },
            new() { Name = "Empty", Type = typeof(VollProcessSchedule) },
        });
    }
}