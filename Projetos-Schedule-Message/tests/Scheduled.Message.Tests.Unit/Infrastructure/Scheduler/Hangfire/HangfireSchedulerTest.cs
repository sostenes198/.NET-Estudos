using FluentAssertions;
using Hangfire;
using Hangfire.Common;
using Hangfire.States;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Moq;
using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Scheduler.Hangfire;

public class HangfireSchedulerTest : IDisposable
{
    public class InputParamUnitTest(string Prop1, string Prop2);

    public class SchedulerHandlerUnitTest : ISchedulerHandler<InputParamUnitTest>
    {
        public Task ExecuteAsync(InputParamUnitTest? input, CancellationToken token = default)
        {
            return Task.CompletedTask;
        }
    }

    public class
        HangifireCallbackHandlerUnitTest : IHangifireCallbackHandler<SchedulerHandlerUnitTest, InputParamUnitTest>
    {
        public bool Executed { get; private set; }

        public Task ExecuteAsync(string id, string jobName)
        {
            Executed = true;
            return Task.CompletedTask;
        }
    }

    private readonly HangfireConfigurations _hangfireConfigurations;

    private readonly Mock<IHangfireSchedulerRepository<InputParamUnitTest>> _hangfireSchedulerRepositoryMock;
    private readonly Mock<IBackgroundJobClient> _jobClientMock;
    private readonly Mock<ILogger<HangfireScheduler>> _loggerMock;
    private readonly IScheduler _scheduler;

    public HangfireSchedulerTest()
    {
        _hangfireConfigurations = HangfireConfigurationsFaker.Create();

        _hangfireSchedulerRepositoryMock =
            new Mock<IHangfireSchedulerRepository<InputParamUnitTest>>(MockBehavior.Strict);

        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(lnq => lnq.GetService(typeof(IHangfireSchedulerRepository<InputParamUnitTest>)))
            .Returns(_hangfireSchedulerRepositoryMock.Object);

        _jobClientMock = new Mock<IBackgroundJobClient>(MockBehavior.Strict);
        _loggerMock = new Mock<ILogger<HangfireScheduler>>();

        _scheduler = new HangfireScheduler(serviceProviderMock.Object,
            new OptionsWrapper<HangfireConfigurations>(_hangfireConfigurations),
            _jobClientMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Should_Schedule_Successfully()
    {
        // Arrange
        var jobName = "JOB_NAME";
        var input = new InputParamUnitTest("PROP_1", "PROP_2");
        var delayToWait = TimeSpan.FromMinutes(5);
        var token = CancellationToken.None;
        var hangifireCallbackHandlerUnitTest = new HangifireCallbackHandlerUnitTest();

        _hangfireSchedulerRepositoryMock.Setup(lnq => lnq.SaveParamsAsync(
            It.IsAny<HangfireParams<InputParamUnitTest>>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        _jobClientMock.Setup(lnq =>
                lnq.Create(It.IsAny<Job>(), It.IsAny<IState>())).Returns("JOB_ID")
            .Callback((Job job, IState state) =>
            {
                job.Method.Invoke(hangifireCallbackHandlerUnitTest, [job.Args[0], job.Args[1]]);
            });

        // Act
        await _scheduler.ScheduleAsync<InputParamUnitTest, SchedulerHandlerUnitTest>(
            jobName, input, delayToWait, token);

        // Assert
        hangifireCallbackHandlerUnitTest.Executed.Should().BeTrue();
        _hangfireSchedulerRepositoryMock.Verify(lnq => lnq.SaveParamsAsync(MoqAssert.Assert(
                new HangfireParams<InputParamUnitTest>
                {
                    Id = ObjectId.GenerateNewId(),
                    Input = input,
                    ExpireAt = DateTime.UtcNow.AddDays(_hangfireConfigurations.TtlHangfireDocumentInDays)
                }
                , t => t.Excluding(prop => prop.Id).Excluding(prop => prop.ExpireAt)),
            It.IsAny<CancellationToken>()), Times.Once);
        _jobClientMock.Verify(lnq => lnq.Create(It.IsAny<Job>(), It.IsAny<IState>()), Times.Once);
        _loggerMock.VerifyLog(lnq => lnq.LogInformation("Request scheduled successfully {id}", jobName));
    }

    public void Dispose()
    {
        _jobClientMock.VerifyAll();
        _jobClientMock.VerifyNoOtherCalls();
        _hangfireSchedulerRepositoryMock.VerifyAll();
        _hangfireSchedulerRepositoryMock.VerifyNoOtherCalls();
    }
}