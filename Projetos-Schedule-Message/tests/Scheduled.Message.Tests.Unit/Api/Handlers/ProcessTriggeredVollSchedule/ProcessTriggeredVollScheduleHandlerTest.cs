using Microsoft.Extensions.Logging;
using Moq;
using Scheduled.Message.Api.Handlers.ProcessTriggeredVollSchedule;
using Scheduled.Message.Application.Boundaries.Scheduler.Handlers;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;
using Scheduled.Message.Domain.ScheduleVollProcess;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

// ReSharper disable NotAccessedPositionalProperty.Global

namespace Scheduled.Message.Tests.Unit.Api.Handlers.ProcessTriggeredVollSchedule;

public class ProcessTriggeredVollScheduleHandlerTest : IDisposable
{
    private readonly Mock<ILogger<ProcessTriggeredVollScheduleHandler>> _loggerMock;
    private readonly Mock<IUseCaseManager> _useCaseManagerMock;
    private readonly Mock<ITriggeredVollScheduleOutput> _outputMock;
    private readonly IProcessTriggeredVollScheduleHandler _handler;

    public ProcessTriggeredVollScheduleHandlerTest()
    {
        _loggerMock = new Mock<ILogger<ProcessTriggeredVollScheduleHandler>>();
        _useCaseManagerMock = new Mock<IUseCaseManager>(MockBehavior.Strict);
        _outputMock = new Mock<ITriggeredVollScheduleOutput>(MockBehavior.Strict);
        _handler = new ProcessTriggeredVollScheduleHandler(
            _loggerMock.Object,
            _useCaseManagerMock.Object,
            _outputMock.Object);
    }

    public record ProcessTriggeredScheduleScenario(
        string Name,
        VollProcessSchedule? Model,
        TriggeredVollScheduleUseCaseInput Expected);

    public static IEnumerable<object[]> ScenariosProcessTriggeredSchedule()
    {
        var input = VollProcessScheduleFaker.Create();
        yield return
        [
            new ProcessTriggeredScheduleScenario(
                "When not null input",
                input,
                new TriggeredVollScheduleUseCaseInput(
                    input.When,
                    input.JobName,
                    input.TopicName,
                    input.Key,
                    input.Value,
                    input.Headers
                ))
        ];

        yield return
        [
            new ProcessTriggeredScheduleScenario(
                "When null input",
                default,
                new TriggeredVollScheduleUseCaseInput(
                    default,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    string.Empty
                ))
        ];
    }

    [Theory]
    [MemberData(nameof(ScenariosProcessTriggeredSchedule))]
    public async Task Shuld_Process_Triggered_Schedule_Successfully(ProcessTriggeredScheduleScenario scenario)
    {
        // Arrange
        var token = CancellationToken.None;

        _useCaseManagerMock.Setup(lnq => lnq.ExecuteAsync(
            It.IsAny<TriggeredVollScheduleUseCaseInput>(),
            It.IsAny<ITriggeredVollScheduleOutput>(),
            It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _outputMock.Setup(lnq => lnq.Success());

        // Act
        await _handler.ExecuteAsync(scenario.Model, token);

        // Assert
        _useCaseManagerMock.Verify(lnq => lnq.ExecuteAsync(
            MoqAssert.Assert(
                new TriggeredVollScheduleUseCaseInput(
                    scenario.Expected.When,
                    scenario.Expected.JobName,
                    scenario.Expected.TopicName,
                    scenario.Expected.Key,
                    scenario.Expected.Value,
                    scenario.Expected.Headers
                )),
            It.IsAny<ITriggeredVollScheduleOutput>(),
            token
        ), Times.Once);
        _outputMock.Verify(lnq => lnq.Success(), Times.Once);
        _loggerMock.VerifyLog(lnq =>
            lnq.LogInformation("Initialize UseCase TriggeredVollSchedule with input {Input}", scenario.Model));
        _loggerMock.VerifyLog(lnq => lnq.LogInformation("End UseCase TriggeredVollSchedule"));
    }

    public void Dispose()
    {
        _useCaseManagerMock.VerifyAll();
        _useCaseManagerMock.VerifyNoOtherCalls();

        _outputMock.VerifyAll();
        _outputMock.VerifyNoOtherCalls();
    }
}