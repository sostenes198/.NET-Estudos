using FluentAssertions;
using Scheduled.Message.Api.Presenters.Handlers.TriggeredVollSchedule;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

namespace Scheduled.Message.Tests.Unit.Api.Presenters.Handlers.TriggeredVollSchedule;

public class TriggeredVollSchedulePresenterTest
{
    private readonly ITriggeredVollScheduleOutput _presenter;

    public TriggeredVollSchedulePresenterTest()
    {
        _presenter = new TriggeredVollSchedulePresenter();
    }

    [Fact]
    public void Should_Validate_Success_Successfully()
    {
        // Arrange
        Action act = () => _presenter.Success();

        // Act - Assert
        act.Should().NotThrow();
    }
}