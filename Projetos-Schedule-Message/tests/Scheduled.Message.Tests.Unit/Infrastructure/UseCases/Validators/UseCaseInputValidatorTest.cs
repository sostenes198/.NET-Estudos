using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;
using Scheduled.Message.Infrastructure.UseCases.Validators;
using Scheduled.Message.Tests.Base.Utils;

// ReSharper disable StructuredMessageTemplateProblem

namespace Scheduled.Message.Tests.Unit.Infrastructure.UseCases.Validators;

[SuppressMessage("Reliability", "CA2017:Incompatibilidade de contagem de parâmetros")]
public class UseCaseInputValidatorTest : IDisposable
{
    internal class UseCaseInput : IUseCaseInput
    {
    }

    private readonly Mock<IValidator<UseCaseInput>> _validatorMock;
    private readonly Mock<ILogger<UseCaseInputValidator<UseCaseInput>>> _loggerMock;
    private readonly IUseCaseInputValidator<UseCaseInput> _useCaseInputValidator;

    private readonly UseCaseInput _input;

    public UseCaseInputValidatorTest()
    {
        _validatorMock = new Mock<IValidator<UseCaseInput>>();
        _loggerMock = new Mock<ILogger<UseCaseInputValidator<UseCaseInput>>>();
        _useCaseInputValidator = new UseCaseInputValidator<UseCaseInput>(_validatorMock.Object, _loggerMock.Object);

        _input = new UseCaseInput();
    }

    [Fact]
    public void Should_Validate_Input_Valid()
    {
        // Arrange
        _validatorMock.Setup(lnq => lnq.Validate(It.IsAny<UseCaseInput>())).Returns(new ValidationResult());

        // Act
        var result = _useCaseInputValidator.Validate(_input, out var notificationErrors, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        notificationErrors.Should().BeEquivalentTo(NotificationsInputError.Empty);
        ValidateMocks(1, 0);
    }

    [Fact]
    public void Should_Validate_Input_Invalid()
    {
        // Arrange
        _validatorMock.Setup(lnq => lnq.Validate(It.IsAny<UseCaseInput>()))
            .Returns(new ValidationResult(
                new[]
                {
                    new ValidationFailure("Prop1", "FAILED_1"),
                    new ValidationFailure("Prop2", "FAILED_1"),
                    new ValidationFailure("Prop2", "FAILED_2")
                }));

        var expectedErrors = new NotificationsInputError();
        expectedErrors.Add("Prop1", "FAILED_1");
        expectedErrors.Add("Prop2", "FAILED_1");
        expectedErrors.Add("Prop2", "FAILED_2");

        // Act
        var result = _useCaseInputValidator.Validate(_input, out var notificationErrors,
            CancellationToken.None);

        // Assert
        result.Should().BeFalse();
        notificationErrors.Should().BeEquivalentTo(expectedErrors);
        ValidateMocks(1, 1);
    }

    private void ValidateMocks(int countValidate, int countLogInvalidInput)
    {
        _validatorMock.Verify(lnq => lnq.Validate(MoqAssert.Assert(_input)),
            Times.Exactly(countValidate));
        _loggerMock.VerifyLog(lnq => lnq.LogInformation("Invalid input: {Input}"),
            Times.Exactly(countLogInvalidInput));
    }

    public void Dispose()
    {
        _validatorMock.VerifyAll();
        _validatorMock.VerifyNoOtherCalls();
        _loggerMock.VerifyAll();
        _loggerMock.VerifyNoOtherCalls();
    }
}