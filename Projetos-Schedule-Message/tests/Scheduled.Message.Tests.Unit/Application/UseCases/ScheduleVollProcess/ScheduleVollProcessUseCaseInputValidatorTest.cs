using System.Globalization;
using FluentAssertions;
using FluentValidation;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;

namespace Scheduled.Message.Tests.Unit.Application.UseCases.ScheduleVollProcess;

public class ScheduleVollProcessUseCaseInputValidatorTest
{
    private readonly IValidator<ScheduleVollProcessUseCaseInput> _validator;

    public ScheduleVollProcessUseCaseInputValidatorTest()
    {
        ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("en");
        _validator = new ScheduleVollProcessUseCaseInputValidator();
    }

    [Fact]
    public void Should_Validate_Input_Successfully()
    {
        // Arrange
        var input = ScheduleVollProcessUseCaseInputFaker.Create();

        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Validate_Input_With_Error_When_Date_When_Invalid()
    {
        // Arrange
        DateTime date = default!;
        var input = ScheduleVollProcessUseCaseInputFaker.Create(date);

        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        var error = result.Errors.Single();
        error.ErrorCode.Should().Be("NotEmptyValidator");
        error.ErrorMessage.Should().Be("'When' must not be empty.");
        error.PropertyName.Should().Be("When");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Validate_Input_With_Error_When_Job_Name_Invalid(string? jobName)
    {
        // Arrange
        var input = ScheduleVollProcessUseCaseInputFaker.Create(jobName: jobName!);

        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        var error = result.Errors.Single();
        error.ErrorCode.Should().Be("NotEmptyValidator");
        error.ErrorMessage.Should().Be("'Job Name' must not be empty.");
        error.PropertyName.Should().Be("JobName");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Validate_Input_With_Error_When_Topic_Name_Invalid(string? topicName)
    {
        // Arrange
        var input = ScheduleVollProcessUseCaseInputFaker.Create(topicName: topicName!);

        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        var error = result.Errors.Single();
        error.ErrorCode.Should().Be("NotEmptyValidator");
        error.ErrorMessage.Should().Be("'Topic Name' must not be empty.");
        error.PropertyName.Should().Be("TopicName");
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Validate_Input_With_Error_When_Key_Invalid(string? key)
    {
        // Arrange
        var input = ScheduleVollProcessUseCaseInputFaker.Create(key: key!);

        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        var error = result.Errors.Single();
        error.ErrorCode.Should().Be("NotEmptyValidator");
        error.ErrorMessage.Should().Be("'Key' must not be empty.");
        error.PropertyName.Should().Be("Key");
    }
}