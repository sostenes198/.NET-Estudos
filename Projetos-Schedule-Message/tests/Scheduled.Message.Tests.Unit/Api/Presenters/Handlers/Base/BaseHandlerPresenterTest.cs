using FluentAssertions;
using Scheduled.Message.Api.Presenters.Handlers.Base;
using Scheduled.Message.Application.Boundaries.UseCases;

namespace Scheduled.Message.Tests.Unit.Api.Presenters.Handlers.Base;

public class BaseHandlerPresenterTest
{
    public class UseCaseInputUnitTest : IUseCaseInput;

    private readonly BaseHandlerPresenter _presenter;

    public BaseHandlerPresenterTest()
    {
        _presenter = new BaseHandlerPresenter();
    }

    [Fact]
    public void Should_Validate_Invalid_Input_Successfully()
    {
        // Arrange
        Action act = () => _presenter.InvalidInput<UseCaseInputUnitTest>(null!, null!);

        // Act - Assert
        act.Should().NotThrow();
    }

    [Fact]
    public void Should_Validate_Hanlder_Error_Successfully()
    {
        // Arrange
        Action act = () => _presenter.HandlerError<UseCaseInputUnitTest>(null!, null!);

        // Act - Assert
        act.Should().NotThrow();
    }
}