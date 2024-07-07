using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Outputs;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;
using Scheduled.Message.Infrastructure.UseCases;
using Scheduled.Message.Tests.Base.Utils;

// ReSharper disable StructuredMessageTemplateProblem

namespace Scheduled.Message.Tests.Unit.Infrastructure.UseCases;

[SuppressMessage("Reliability", "CA2017:Incompatibilidade de contagem de parâmetros")]
public class UseCaseManagerTest : IDisposable
{
    // ReSharper disable once MemberCanBePrivate.Global
    internal class UseCaseInput : IUseCaseInput
    {
    }

    // ReSharper disable once MemberCanBePrivate.Global
    internal class UseCaseOutput : IUseCaseOutput
    {
        public bool Executed { get; set; }

        public void InvalidInput<TUseCaseInput>(TUseCaseInput input, NotificationsInputError errors)
            where TUseCaseInput : IUseCaseInput
        {
            Executed = true;
        }
    }

    internal class UseCaseOutputWithValidations : IUseCaseOutputInvalidInput, IUseCaseOutputHandlerError
    {
        public bool ExecutedInvalidInput { get; set; }
        public bool ExecutedUnhandledError { get; set; }

        public void InvalidInput<TUseCaseInput>(TUseCaseInput input, NotificationsInputError errors)
            where TUseCaseInput : IUseCaseInput
        {
            ExecutedInvalidInput = true;
        }

        public void HandlerError<TUseCaseInput>(TUseCaseInput input, Exception error)
            where TUseCaseInput : IUseCaseInput
        {
            ExecutedUnhandledError = true;
        }
    }

    private readonly UseCaseInput _input;
    private readonly UseCaseOutput _output;
    private readonly UseCaseOutputWithValidations _outputWithValidations;

    private readonly Mock<ILogger<UseCaseManager>> _loggerMock;
    private readonly Mock<IUseCaseInputValidator<UseCaseInput>> _useCaseInputValidatorMock;
    private readonly Mock<IUseCase<UseCaseInput, UseCaseOutput>> _useCaseMock;
    private readonly Mock<IUseCase<UseCaseInput, UseCaseOutputWithValidations>> _useCaseMockWithValidations;

    private readonly IUseCaseManager _useCaseManager;

    public UseCaseManagerTest()
    {
        _input = new UseCaseInput();
        _output = new UseCaseOutput();
        _outputWithValidations = new UseCaseOutputWithValidations();

        _loggerMock = new Mock<ILogger<UseCaseManager>>();
        _useCaseInputValidatorMock = new Mock<IUseCaseInputValidator<UseCaseInput>>();
        _useCaseMockWithValidations = new Mock<IUseCase<UseCaseInput, UseCaseOutputWithValidations>>();
        _useCaseMock = new Mock<IUseCase<UseCaseInput, UseCaseOutput>>();

        var serviceProviderMock = new Mock<IServiceProvider>();

        serviceProviderMock.Setup(lnq => lnq.GetService(typeof(IUseCaseInputValidator<UseCaseInput>)))
            .Returns(_useCaseInputValidatorMock.Object);
        serviceProviderMock.Setup(lnq => lnq.GetService(typeof(IUseCase<UseCaseInput, UseCaseOutput>)))
            .Returns(_useCaseMock.Object);
        serviceProviderMock.Setup(lnq => lnq.GetService(typeof(IUseCase<UseCaseInput, UseCaseOutputWithValidations>)))
            .Returns(_useCaseMockWithValidations.Object);

        _useCaseManager = new UseCaseManager(serviceProviderMock.Object, _loggerMock.Object);
    }


    [Fact]
    public async Task Should_Execute_Use_Case_Manager_Successfully_Without_Input_Validation()
    {
        // Arrange
        _useCaseMock.Setup(lnq =>
                lnq.ExecuteAsync(It.IsAny<UseCaseInput>(), It.IsAny<UseCaseOutput>(), CancellationToken.None))
            .Returns(Task.CompletedTask);

        // Act
        await _useCaseManager.ExecuteAsync(_input, _output, CancellationToken.None);

        // Assert
        _output.Executed.Should().BeFalse();
        AssertMocks(0, 1, 0, false, false);
    }

    [Fact]
    public async Task Should_Execute_Use_Case_Manager_Successfully_With_Input_Validation()
    {
        // Arrange
        var notificationErrors = NotificationsInputError.Empty;

        _useCaseMockWithValidations.Setup(lnq =>
                lnq.ExecuteAsync(It.IsAny<UseCaseInput>(), It.IsAny<UseCaseOutputWithValidations>(),
                    CancellationToken.None))
            .Returns(Task.CompletedTask);

        _useCaseInputValidatorMock.Setup(lnq =>
            lnq.Validate(It.IsAny<UseCaseInput>(), out notificationErrors, CancellationToken.None)).Returns(true);

        // Act
        await _useCaseManager.ExecuteAsync(_input, _outputWithValidations, CancellationToken.None);

        // Assert
        notificationErrors.Should().BeEquivalentTo(NotificationsInputError.Empty);
        _output.Executed.Should().BeFalse();
        AssertMocks(1, 0, 1, false, false);
    }

    [Fact]
    public async Task Should_Execute_Use_Case_Manager_With_Input_Validation_And_Input_Invalid()
    {
        // Arrange
        var notificationErrors = NotificationsInputError.Empty;

        _useCaseInputValidatorMock.Setup(lnq =>
            lnq.Validate(It.IsAny<UseCaseInput>(), out notificationErrors, CancellationToken.None)).Returns(false);

        // Act
        await _useCaseManager.ExecuteAsync(_input, _outputWithValidations, CancellationToken.None);

        // Assert
        notificationErrors.Should().BeEquivalentTo(NotificationsInputError.Empty);
        _output.Executed.Should().BeFalse();
        AssertMocks(1, 0, 0, true, false);
    }

    [Fact]
    public async Task Should_Execute_Use_Case_Manager_And_Thrown_Exception()
    {
        // Arrange
        _useCaseMock.Setup(lnq =>
                lnq.ExecuteAsync(It.IsAny<UseCaseInput>(), It.IsAny<UseCaseOutput>(), CancellationToken.None))
            .Throws(() => new Exception("FAIL"));

        // Act
        Func<Task> act = () => _useCaseManager.ExecuteAsync(_input, _output, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("FAIL");
        AssertMocks(0, 1, 0, false, false);
    }

    [Fact]
    public async Task Should_Execute_Use_Case_Manager_And_Handle_Exception_When_Use_Case_Is_Handler_Error()
    {
        // Arrange
        var notificationErrors = NotificationsInputError.Empty;

        _useCaseMockWithValidations.Setup(lnq =>
                lnq.ExecuteAsync(It.IsAny<UseCaseInput>(), It.IsAny<UseCaseOutputWithValidations>(),
                    CancellationToken.None))
            .Throws(() => new Exception("FAIL"));

        _useCaseInputValidatorMock.Setup(lnq =>
            lnq.Validate(It.IsAny<UseCaseInput>(), out notificationErrors, CancellationToken.None)).Returns(true);

        // Act
        Func<Task> act = () => _useCaseManager.ExecuteAsync(_input, _outputWithValidations, CancellationToken.None);

        // Assert
        notificationErrors.Should().BeEquivalentTo(NotificationsInputError.Empty);
        await act.Should().NotThrowAsync();
        AssertMocks(1, 0, 1, false, true);
        _loggerMock.VerifyLog(lnq => lnq.LogError(It.IsAny<Exception>(), "Exception: {message}",
            It.IsAny<object[]>()), Times.Once);
    }

    private void AssertMocks(int countValidate,
        int countExecuteAsync,
        int countExecuteAsyncWithValidations,
        bool executedInvalidInput,
        bool executedUnhandledError)
    {
        _outputWithValidations.ExecutedInvalidInput.Should().Be(executedInvalidInput);
        _outputWithValidations.ExecutedUnhandledError.Should().Be(executedUnhandledError);
        _useCaseInputValidatorMock.Verify(lnq =>
                lnq.Validate(MoqAssert.Assert(_input), out It.Ref<NotificationsInputError>.IsAny,
                    CancellationToken.None),
            Times.Exactly(countValidate));
        _useCaseMock.Verify(lnq =>
                lnq.ExecuteAsync(MoqAssert.Assert(_input), MoqAssert.Assert(_output), CancellationToken.None),
            Times.Exactly(countExecuteAsync));
        _useCaseMockWithValidations.Verify(lnq =>
                lnq.ExecuteAsync(MoqAssert.Assert(_input), MoqAssert.Assert(_outputWithValidations),
                    CancellationToken.None),
            Times.Exactly(countExecuteAsyncWithValidations));
    }

    public void Dispose()
    {
        _useCaseInputValidatorMock.VerifyAll();
        _useCaseInputValidatorMock.VerifyNoOtherCalls();
        _useCaseMock.VerifyAll();
        _useCaseMock.VerifyNoOtherCalls();
        _useCaseMockWithValidations.VerifyAll();
        _useCaseMockWithValidations.VerifyNoOtherCalls();
    }
}