using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Scheduled.Message.Api.Controllers.V1;
using Scheduled.Message.Api.Presenters.Http.ScheduleVollProcess;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Api.Controllers.V1;

public class VollScheduleControllerTest : IDisposable
{
    private readonly Mock<ILogger<VollScheduleController>> _loggerMock;
    private readonly Mock<IUseCaseManager> _managerMock;
    private readonly ScheduleVollProcessPresenter _presenter;
    private readonly VollScheduleController _controller;

    public VollScheduleControllerTest()
    {
        _loggerMock = new Mock<ILogger<VollScheduleController>>();
        _managerMock = new Mock<IUseCaseManager>(MockBehavior.Strict);
        _presenter = new ScheduleVollProcessPresenter();

        _controller = new VollScheduleController(_loggerMock.Object, _managerMock.Object, _presenter);
    }

    [Fact]
    public async Task Should_Schedule_Successfully()
    {
        // Arrange
        var token = CancellationToken.None;
        var date = DateTime.Now;

        var input = ScheduleVollProcessModelFaker.Create(date);

        _managerMock.Setup(lnq => lnq.ExecuteAsync(
                It.IsAny<ScheduleVollProcessUseCaseInput>(),
                It.IsAny<IScheduleVollProcessUseCaseOutput>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Callback(() => { _presenter.Success(); });

        // Act
        var result = await _controller.ScheduleAsync(input, token);

        // Assert
        result.Should().BeEquivalentTo(new OkResult());
        _managerMock.Verify(lnq => lnq.ExecuteAsync(
            MoqAssert.Assert(new ScheduleVollProcessUseCaseInput(
                input.When,
                input.JobName,
                input.TopicName,
                input.Key.ToString(),
                input.Value.ToString(),
                input.Headers.ToString()
            )),
            It.IsAny<IScheduleVollProcessUseCaseOutput>(),
            token));
        _loggerMock.VerifyLog(lnq =>
            lnq.LogInformation("Initialize UseCase ScheduleVollProcess with input {Input}", input));
        _loggerMock.VerifyLog(lnq => lnq.LogInformation("End UseCase ScheduleVollProcess"));
    }

    public void Dispose()
    {
        _managerMock.VerifyAll();
        _managerMock.VerifyNoOtherCalls();
    }
}