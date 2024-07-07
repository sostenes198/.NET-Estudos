using FluentAssertions;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

namespace Scheduled.Message.Tests.Unit.Infrastructure.Scheduler.Hangfire;

public class HangfireSchedulerRepositoryTest : IDisposable
{
    public class HangfireSchedulerRepositoryInputUnitTest(string Prop1, string Prop2)
    {
        public string Prop1 { get; } = Prop1;
        public string Prop2 { get; } = Prop2;
    }

    private readonly HangfireParams<HangfireSchedulerRepositoryInputUnitTest> _hangfireParams;
    private readonly CancellationToken _token;

    private readonly MongoConfigurations _mongoConfigurations;
    private readonly Mock<IMongoDatabaseClient<AppMongoDatabaseHangfire>> _mongoDatabaseClientMock;

    private readonly IHangfireSchedulerRepository<HangfireSchedulerRepositoryInputUnitTest> _repository;

    public HangfireSchedulerRepositoryTest()
    {
        _hangfireParams = new HangfireParams<HangfireSchedulerRepositoryInputUnitTest>()
        {
            Id = ObjectId.GenerateNewId(),
            ExpireAt = new DateTime(),
            Input = new HangfireSchedulerRepositoryInputUnitTest("PROP_1", "PROP_2")
        };
        _token = CancellationToken.None;

        _mongoConfigurations = MongoConfigurationsFaker.Create();
        _mongoDatabaseClientMock = new Mock<IMongoDatabaseClient<AppMongoDatabaseHangfire>>(MockBehavior.Strict);
        _repository = new HangfireSchedulerRepository<HangfireSchedulerRepositoryInputUnitTest>(
            _mongoDatabaseClientMock.Object,
            new OptionsWrapper<MongoConfigurations>(_mongoConfigurations));
    }

    [Fact]
    public async Task Should_Save_Hangfire_Params_Successfully()
    {
        // Arrange
        _mongoDatabaseClientMock.Setup(lnq => lnq.CreateAsync(It.IsAny<string>(),
            It.IsAny<HangfireParams<HangfireSchedulerRepositoryInputUnitTest>>(),
            It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        await _repository.SaveParamsAsync(_hangfireParams, _token);

        // Assert
        _mongoDatabaseClientMock.Verify(lnq => lnq.CreateAsync(
            _mongoConfigurations.Hangfire.HangfireParametersCollection,
            MoqAssert.Assert(_hangfireParams),
            default,
            _token
        ));
    }

    [Fact]
    public async Task Should_Recovery_Hangfire_Params_Successfully()
    {
        // Arrange
        _mongoDatabaseClientMock.Setup(lnq => lnq.GetAsync(It.IsAny<string>(),
            It.IsAny<FilterDefinition<HangfireParams<HangfireSchedulerRepositoryInputUnitTest>>>(),
            It.IsAny<FindOptions<HangfireParams<HangfireSchedulerRepositoryInputUnitTest>>>(),
            It.IsAny<CancellationToken>())).ReturnsAsync(_hangfireParams);

        // Act
        var result = await _repository.RecoveryParamsAsync(_hangfireParams.Id.ToString(), _token);

        // Assert
        result.Should().BeEquivalentTo(_hangfireParams);
        _mongoDatabaseClientMock.Verify(lnq => lnq.GetAsync(
            _mongoConfigurations.Hangfire.HangfireParametersCollection,
            It.IsAny<FilterDefinition<HangfireParams<HangfireSchedulerRepositoryInputUnitTest>>>(),
            default,
            _token
        ));
    }

    public void Dispose()
    {
        _mongoDatabaseClientMock.VerifyAll();
        _mongoDatabaseClientMock.VerifyNoOtherCalls();
    }
}