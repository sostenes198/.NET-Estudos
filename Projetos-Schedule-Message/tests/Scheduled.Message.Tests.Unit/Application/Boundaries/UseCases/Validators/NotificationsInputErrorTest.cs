using FluentAssertions;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;

namespace Scheduled.Message.Tests.Unit.Application.Boundaries.UseCases.Validators;

public class NotificationsInputErrorTest
{
    [Fact]
    public void Should_Validate_Notifications_Input_Error_Empty()
    {
        // Arrange - Assert - Assert
        NotificationsInputError.Empty.Should().BeEquivalentTo(new NotificationsInputError());
    }

    [Fact]
    public void Should_Validate_Notifications_Input_Error_With_Errors()
    {
        // Arrange 
        var errors = new NotificationsInputError();
        errors.Add("KEY_1", "ERROR_1");
        errors.Add("KEY_1", "ERROR_2");
        errors.Add("KEY_2", "ERROR_1");
        errors.Add("KEY_3", "ERROR_1");
        errors.Add("KEY_3", "ERROR_2");
        errors.Add("KEY_3", "ERROR_3");

        var errorsExpected = new Dictionary<string, string[]>
        {
            { "KEY_1", ["ERROR_1", "ERROR_2"] },
            { "KEY_2", ["ERROR_1"] },
            { "KEY_3", ["ERROR_1", "ERROR_2", "ERROR_3"] },
        };

        // Act - Assert
        errors.Errors.Should().BeEquivalentTo(errorsExpected);
    }
}