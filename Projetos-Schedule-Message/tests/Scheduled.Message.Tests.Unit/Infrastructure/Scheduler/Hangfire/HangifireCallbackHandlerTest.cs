using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Scheduler.Hangfire;

public class HangifireCallbackHandlerTest : IDisposable
{
    public class HangifireCallbackHandlerUnitTestInput;

    public class
        HangifireCallbackHandlerUnitTestSchedulerHandler : ISchedulerHandler<HangifireCallbackHandlerUnitTestInput>
    {
        public bool Executed { get; private set; }

        public Task ExecuteAsync(HangifireCallbackHandlerUnitTestInput? input, CancellationToken token = default)
        {
            Executed = true;
            return Task.CompletedTask;
        }
    }

    private static readonly ObjectId Id = ObjectId.GenerateNewId();
    private const string JobName = "JOB_NAME";

    private readonly HangfireParams<HangifireCallbackHandlerUnitTestInput> _hangfireParams;

    private readonly HangifireCallbackHandlerUnitTestSchedulerHandler _schedulerHandler;

    private readonly Mock<IHangfireSchedulerRepository<HangifireCallbackHandlerUnitTestInput>> _schedulerRepositoryMock;

    private readonly Mock<ILogger<HangifireCallbackHandler<HangifireCallbackHandlerUnitTestSchedulerHandler,
        HangifireCallbackHandlerUnitTestInput>>> _loggerMock;

    private readonly
        IHangifireCallbackHandler<HangifireCallbackHandlerUnitTestSchedulerHandler,
            HangifireCallbackHandlerUnitTestInput> _hangifireCallbackHandler;


    public HangifireCallbackHandlerTest()
    {
        _hangfireParams = new HangfireParams<HangifireCallbackHandlerUnitTestInput>
        {
            Id = Id,
            ExpireAt = new DateTime(),
            Input = new HangifireCallbackHandlerUnitTestInput()
        };

        _schedulerHandler = new HangifireCallbackHandlerUnitTestSchedulerHandler();

        _schedulerRepositoryMock =
            new Mock<IHangfireSchedulerRepository<HangifireCallbackHandlerUnitTestInput>>(MockBehavior.Strict);

        _loggerMock =
            new Mock<ILogger<HangifireCallbackHandler<HangifireCallbackHandlerUnitTestSchedulerHandler,
                HangifireCallbackHandlerUnitTestInput>>>();

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(lnq =>
                lnq.GetService(typeof(HangifireCallbackHandlerUnitTestSchedulerHandler)))
            .Returns(_schedulerHandler);


        _hangifireCallbackHandler =
            new HangifireCallbackHandler<HangifireCallbackHandlerUnitTestSchedulerHandler,
                HangifireCallbackHandlerUnitTestInput>(serviceProviderMock.Object,
                _schedulerRepositoryMock.Object,
                _loggerMock.Object);
    }

    [Fact]
    public async Task Should_Execute_Hangfire_Callback_Handler_Successfully()
    {
        // Arrange
        _schedulerRepositoryMock.Setup(lnq => lnq.RecoveryParamsAsync(
            It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(_hangfireParams);

        // Act
        await _hangifireCallbackHandler.ExecuteAsync(Id.ToString(), JobName);

        // Assert
        _schedulerHandler.Executed.Should().BeTrue();
        _schedulerRepositoryMock.Verify(lnq =>
            lnq.RecoveryParamsAsync(_hangfireParams.Id.ToString(), CancellationToken.None), Times.Once);
        _loggerMock.VerifyLog(lnq =>
            lnq.LogInformation("{id} Job execution successfully completed", _hangfireParams.Id.ToString()));
    }

    public void Dispose()
    {
        _schedulerRepositoryMock.VerifyAll();
        _schedulerRepositoryMock.VerifyNoOtherCalls();
    }
}