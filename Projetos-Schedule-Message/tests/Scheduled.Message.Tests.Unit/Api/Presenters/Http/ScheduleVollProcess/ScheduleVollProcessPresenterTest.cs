using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Scheduled.Message.Api.Presenters.Http.ScheduleVollProcess;
using Scheduled.Message.Tests.Unit.Api.Presenters.Http.Base;

namespace Scheduled.Message.Tests.Unit.Api.Presenters.Http.ScheduleVollProcess;

public class ScheduleVollProcessPresenterTest : BaseHttpPresenterTest<ScheduleVollProcessPresenter>
{
    public override ScheduleVollProcessPresenter Presenter { get; }

    public ScheduleVollProcessPresenterTest()
    {
        Presenter = new ScheduleVollProcessPresenter();
    }

    [Fact]
    public void Should_Validate_Success_Successfully()
    {
        // Arrange - Act
        Presenter.Success();

        // Assert
        var result = (OkResult)Presenter.Result();
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(StatusCodes.Status200OK);
    }
}