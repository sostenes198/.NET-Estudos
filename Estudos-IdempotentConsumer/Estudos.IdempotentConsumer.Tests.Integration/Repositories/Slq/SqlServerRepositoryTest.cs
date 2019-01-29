using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Options;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using Estudos.IdempotentConsumer.Repositories.Slq;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Tests.Integration.Helpers;
using Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq.BaseInserts;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq;

public class SqlServerRepositoryTest : BaseSqlRepositoryTest
{
    private readonly IDataContext _dataContext;
    private readonly IRepository _repository;

    public const string IntegrationTestInstanceId = "Integration-Test-IntanceId";
    public const string IntegrationIdempotencyKeyDefault = "2c5d0166-cbd0-44a0-81d2-9b6670ac4dde";

    public SqlServerRepositoryTest()
    {
        ConfigureServices += (_, collection) =>
        {
            collection.Configure<InMemoryCircularBufferRepositoryOptions>(opt => { opt.MaxItemsBuffer = 10; });
            collection.TryAddSingleton<IInMemoryCircularBufferRepository, InMemoryCircularBufferRepository>();
            collection.TryAddScoped<IRepository, SqlServerRepository>();
        };

        Configure += builder => { builder.ApplicationServices.GetRequiredService<IInMemoryCircularBufferRepository>().SetEntries(BaseInsert.Instance2Item1.InstanceId, new[] {BaseInsert.Instance2Item2, BaseInsert.Instance2Item3}); };

        _repository = ServiceProvider.GetRequiredService<IRepository>();
        _dataContext = ServiceProvider.GetRequiredService<IDataContext>();
    }

    protected override void DisposeBase()
    {
        Task.Run(async () => await CleanToTestAsync()).Wait();
    }
    
    private async Task CleanToTestAsync()
    {
        await EntrySqlHelper.DeleteAsync(_dataContext, IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);
    }

    [Fact(DisplayName = "Deve obter Entry")]
    public async Task ShouldGetEntry()
    {
        // arrange
        const string instanceId = BaseInsert.InstanceId1;
        var idempotencyKey = BaseInsert.Instance1Item3.IdempotencyKey;
        var expectedResult = BaseInsert.Instance1Item3;

        // act
        var entry = await _repository.GetEntryAsync(instanceId, idempotencyKey);

        // assert
        entry.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = "Deve obter Entries")]
    [InlineData(BaseInsert.InstanceId1)]
    [InlineData(BaseInsert.InstanceId2)]
    public async Task ShouldGetEntries(string instanceId)
    {
        // arrange
        var expectedResult = BaseInsert.Entries.Where(lnq => lnq.InstanceId == instanceId);

        // act
        var result = await _repository.GetEntriesAsync(instanceId, 10);

        // assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = "Deve validar que contém Entry no repositório")]
    [MemberData(nameof(ScenariosContainsEntryInRepository))]
    public async Task ShouldValidatContainsEntryInRepository(string instanceId, string idempotencyKey, bool expectedResult)
    {
        // arrange - act
        var result = await _repository.ContainsAsync(instanceId, idempotencyKey);

        // assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Deve adicionar Entry")]
    public async Task ShouldAddEntry()
    {
        // arrange
        await CleanToTestAsync();

        var entry = new Entry(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault, RepositoryEntryState.Reserved, DateTime.Now.Date);

        // act
        await _repository.AddOrUpdateAsync(entry);

        // assert
        var bufferRepository = ServiceProvider.GetRequiredService<IInMemoryCircularBufferRepository>();

        var repositoryResult = await EntrySqlHelper.GetAsync(_dataContext, IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);
        var inMemoryResult = await bufferRepository.GetEntryAsync(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);

        repositoryResult.Should().BeEquivalentTo(entry);
        inMemoryResult.Should().BeEquivalentTo(entry);
    }

    [Fact(DisplayName = "Deve atualizar entry")]
    public async Task ShouldUpdateEntry()
    {
        // arrange
        await CleanToTestAsync();

        var entry = new Entry(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault, RepositoryEntryState.Reserved, DateTime.Now.Date);
        
        await _repository.AddOrUpdateAsync(entry);
        
        var entryToUpdate = new Entry(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault, RepositoryEntryState.Committed, DateTime.Now.Date);
        
        
        // act
        await _repository.AddOrUpdateAsync(entryToUpdate);
        
        // assert
        var bufferRepository = ServiceProvider.GetRequiredService<IInMemoryCircularBufferRepository>();

        var repositoryResult = await EntrySqlHelper.GetAsync(_dataContext, IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);
        var inMemoryResult = await bufferRepository.GetEntryAsync(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);

        repositoryResult.Should().BeEquivalentTo(entryToUpdate);
        inMemoryResult.Should().BeEquivalentTo(entryToUpdate);
    }

    [Fact(DisplayName = "Deve remove Entry")]
    public async Task ShouldRemoveEntry()
    {
        // arrange
        await CleanToTestAsync();

        var entry = new Entry(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault, RepositoryEntryState.Reserved, DateTime.Now.Date);
        
        await _repository.AddOrUpdateAsync(entry);
        
        // act
        await _repository.RemoveAsync(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);

        // assert
        var bufferRepository = ServiceProvider.GetRequiredService<IInMemoryCircularBufferRepository>();

        var repositoryResult = await EntrySqlHelper.GetAsync(_dataContext, IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);
        var inMemoryResult = await bufferRepository.GetEntryAsync(IntegrationTestInstanceId, IntegrationIdempotencyKeyDefault);

        repositoryResult.Should().BeNull();
        inMemoryResult.Should().BeEquivalentTo(Entry.Empty);
    }

    public static IEnumerable<object[]> ScenariosContainsEntryInRepository => new List<object[]>
    {
        new object[] {BaseInsert.Instance1Item1.InstanceId, BaseInsert.Instance1Item1.IdempotencyKey, true},
        new object[] {BaseInsert.Instance2Item3.InstanceId, BaseInsert.Instance2Item3.IdempotencyKey, true},
        new object[] {string.Empty, string.Empty, false}
    };
}