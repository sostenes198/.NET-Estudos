using Moq;
using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Application.Boundaries.Scheduler.Handlers;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;
using Scheduled.Message.Domain.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Application.UseCases.ScheduleVollProcess;

public class ScheduleVollProcessUseCaseTest : IDisposable
{
    private readonly CancellationToken _token;
    private readonly Mock<IScheduler> _schedulerMock;
    private readonly IUseCase<ScheduleVollProcessUseCaseInput, IScheduleVollProcessUseCaseOutput> _useCase;

    public ScheduleVollProcessUseCaseTest()
    {
        _token = CancellationToken.None;
        _schedulerMock = new Mock<IScheduler>(MockBehavior.Strict);
        _useCase = new ScheduleVollProcessUseCase(_schedulerMock.Object);
    }

    [Fact]
    public async Task Should_Execute_Use_Case_Successfully()
    {
        // Arrange
        var input = ScheduleVollProcessUseCaseInputFaker.Create();

        _schedulerMock.Setup(lnq => lnq.ScheduleAsync<VollProcessSchedule, IProcessTriggeredVollScheduleHandler>(
            It.IsAny<string>(), It.IsAny<VollProcessSchedule>(),
            It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var scheduleVollProcessUseCaseOutputMock = new Mock<IScheduleVollProcessUseCaseOutput>(MockBehavior.Strict);
        scheduleVollProcessUseCaseOutputMock.Setup(lnq => lnq.Success());

        var vollProcessScheduleExpecte = new VollProcessSchedule(
            input.When,
            input.JobName,
            input.TopicName,
            input.Key,
            input.Value,
            input.Headers
        );

        // Act
        await _useCase.ExecuteAsync(input, scheduleVollProcessUseCaseOutputMock.Object, _token);

        // Assert
        scheduleVollProcessUseCaseOutputMock.Verify(lnq => lnq.Success(), Times.Once);
        _schedulerMock.Verify(lnq => lnq.ScheduleAsync<VollProcessSchedule, IProcessTriggeredVollScheduleHandler>(
            input.JobName,
            MoqAssert.Assert(vollProcessScheduleExpecte),
            It.IsAny<TimeSpan>(),
            _token
        ));
    }

    public void Dispose()
    {
        _schedulerMock.VerifyAll();
        _schedulerMock.VerifyNoOtherCalls();
    }
}