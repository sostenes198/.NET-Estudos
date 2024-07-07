using FluentAssertions;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Application.UseCases.ScheduleVollProcess;

public class ScheduleVollProcessUseCaseInputTest
{
    [Fact]
    public void Should_Validate_Schedule_Voll_Process_Use_Case_Input()
    {
        // Arrange
        var date = new DateTime();
        var expectedResult = new ScheduleVollProcessUseCaseInput(
            date,
            "JOB_NAME",
            "TOPIC_NAME",
            "KEY",
            "VALUE",
            "HEADERS"
        );

        // Act
        var result = ScheduleVollProcessUseCaseInputFaker.Create(date);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Schedule_Voll_Process_Use_Case_Input_Properties()
    {
        // Arrange - Act - Assert
        typeof(ScheduleVollProcessUseCaseInput).ValidateProperties(new List<AssertProperty>
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