using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduled.Message.Api.Presenters.Base;
using Scheduled.Message.Api.Presenters.Http.Base;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Tests.Base.Fakers;

namespace Scheduled.Message.Tests.Unit.Api.Presenters.Http.Base;

public abstract class BaseHttpPresenterTest<TPresenter>
    where TPresenter : BaseHttpPresenter
{
    private class UseCaseInputUnitTest : IUseCaseInput;

    public abstract TPresenter Presenter { get; }

    [Fact]
    public void Should_Validate_Invalid_Input()
    {
        // Arrange
        var errors = NotificationsInputErrorFaker.Create();

        // Act
        Presenter.InvalidInput<UseCaseInputUnitTest>(null!, errors);

        // Assert
        var result = (BadRequestObjectResult)Presenter.Result();
        result.Should().NotBeNull();

        result.Value.Should().BeEquivalentTo(new NotificationErrorsResponse(errors.Errors));
    }

    [Fact]
    public void Should_Validate_Handler_Error()
    {
        // Arrange - Act
        Presenter.HandlerError<UseCaseInputUnitTest>(null!, null!);

        // Assert
        var result = (StatusCodeResult)Presenter.Result();
        result.Should().NotBeNull();

        result.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}