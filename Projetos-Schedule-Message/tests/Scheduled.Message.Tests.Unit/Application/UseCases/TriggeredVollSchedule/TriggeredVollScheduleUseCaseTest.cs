using Moq;
using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;
using Scheduled.Message.Domain.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Application.UseCases.TriggeredVollSchedule;

public class TriggeredVollScheduleUseCaseTest : IDisposable
{
    private readonly CancellationToken _token;
    private readonly Mock<IVollSchedulerGateway> _vollSchedulerGatewayMock;
    private readonly IUseCase<TriggeredVollScheduleUseCaseInput, ITriggeredVollScheduleOutput> _useCase;

    public TriggeredVollScheduleUseCaseTest()
    {
        _token = CancellationToken.None;
        _vollSchedulerGatewayMock = new Mock<IVollSchedulerGateway>(MockBehavior.Strict);
        _useCase = new TriggeredVollScheduleUseCase(_vollSchedulerGatewayMock.Object);
    }

    [Fact]
    public async Task Should_Execute_Use_Case_Successfully()
    {
        // Arrange
        var input = TriggeredVollScheduleUseCaseInputFaker.Create();

        _vollSchedulerGatewayMock.Setup(lnq => lnq.SendScheduledMessageAsync(
            It.IsAny<VollProcessSchedule>(),
            It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var triggeredVollScheduleOutputMock = new Mock<ITriggeredVollScheduleOutput>(MockBehavior.Strict);
        triggeredVollScheduleOutputMock.Setup(lnq => lnq.Success());

        var vollProcessScheduleExpecte = new VollProcessSchedule(
            input.When,
            input.JobName,
            input.TopicName,
            input.Key,
            input.Value,
            input.Headers
        );

        // Act
        await _useCase.ExecuteAsync(input, triggeredVollScheduleOutputMock.Object, _token);

        // Assert
        triggeredVollScheduleOutputMock.Verify(lnq => lnq.Success(), Times.Once);
        _vollSchedulerGatewayMock.Verify(lnq => lnq.SendScheduledMessageAsync(
            MoqAssert.Assert(vollProcessScheduleExpecte),
            _token
        ));
    }

    public void Dispose()
    {
        _vollSchedulerGatewayMock.VerifyAll();
        _vollSchedulerGatewayMock.VerifyNoOtherCalls();
    }
}