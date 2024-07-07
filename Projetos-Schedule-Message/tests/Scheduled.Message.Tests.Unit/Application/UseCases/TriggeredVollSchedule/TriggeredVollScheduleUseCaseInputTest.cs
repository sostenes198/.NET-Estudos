using FluentAssertions;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Application.UseCases.TriggeredVollSchedule;

public class TriggeredVollScheduleUseCaseInputTest
{
    [Fact]
    public void Should_Validate_Schedule_Voll_Process_Use_Case_Input()
    {
        // Arrange
        var date = new DateTime();
        var expectedResult = new TriggeredVollScheduleUseCaseInput(
            date,
            "JOB_NAME",
            "TOPIC_NAME",
            "KEY",
            "VALUE",
            "HEADERS"
        );

        // Act
        var result = TriggeredVollScheduleUseCaseInputFaker.Create(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Schedule_Voll_Process_Use_Case_Input_Properties()
    {
        // Arrange - Act - Assert
        typeof(TriggeredVollScheduleUseCaseInput).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "When", Type = typeof(DateTime) },
            new() { Name = "JobName", Type = typeof(string) },
            new() { Name = "TopicName", Type = typeof(string) },
            new() { Name = "Key", Type = typeof(string) },
            new() { Name = "Value", Type = typeof(string) },
            new() { Name = "Headers", Type = typeof(string) },
        });
    }
}