using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using Estudos.IdempotentConsumer.Repositories.Slq;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Tests.Unitary.FluentAssertion;
using FluentAssertions;
using Moq;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Repositories.Sql;

public class SqlServerRepositoryTest
{
    private const string InstanceId1 = "InstanceId-1";
    private const string InstanceId2 = "InstanceId-1";
    private const string InstanceId3 = "InstanceId-3";

    private static readonly Entry Instance1Item1 = new(InstanceId1, "Key-1", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Instance1Item2 = new(InstanceId1, "Key-2", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Instance1Item3 = new(InstanceId1, "Key-3", RepositoryEntryState.None, DateTime.Now);

    private static readonly Entry Instance2Item1 = new(InstanceId2, "Key-1", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Instance2Item2 = new(InstanceId2, "Key-2", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Instance2Item3 = new(InstanceId2, "Key-3", RepositoryEntryState.None, DateTime.Now);

    private static readonly Entry Instance3Item1 = new(InstanceId3, "Key-1", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Instance3Item2 = new(InstanceId3, "Key-2", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Instance3Item3 = new(InstanceId3, "Key-3", RepositoryEntryState.None, DateTime.Now);

    private static readonly Entry[] AllItems = {Instance1Item1, Instance1Item2, Instance1Item3, Instance2Item1, Instance2Item2, Instance2Item3, Instance3Item1, Instance3Item2, Instance3Item3};

    private readonly Mock<ISqlService> _sqlServiceMock;
    private readonly Mock<IInMemoryCircularBufferRepository> _inMemoryCircularBufferRepositoryMock;
    private readonly IRepository _repository;

    public SqlServerRepositoryTest()
    {
        _sqlServiceMock = new Mock<ISqlService>();
        _inMemoryCircularBufferRepositoryMock = new Mock<IInMemoryCircularBufferRepository>();
        _repository = new SqlServerRepository(_sqlServiceMock.Object, _inMemoryCircularBufferRepositoryMock.Object);
    }

    [Theory(DisplayName = "Deve obter entry")]
    [MemberData(nameof(ScenariosGetEntry))]
    public async Task ShouldGetEntry(string instanceId, string idempotencyKey, Entry? entry, Entry expectedResult)
    {
        // arrange
        _sqlServiceMock.Setup(lnq => lnq.QueryFirstOrDefaultAsync<Entry>(It.IsAny<string>(), It.IsAny<object?>())).ReturnsAsync(entry);

        var expectedParamQueryFirstOrDefault = new
        {
            instanceId = instanceId,
            idempotencyKey = idempotencyKey
        };

        // act
        var result = await _repository.GetEntryAsync(instanceId, idempotencyKey);

        // assert
        result.Should().BeEquivalentTo(expectedResult);

        _sqlServiceMock.Verify(lnq => lnq.QueryFirstOrDefaultAsync<Entry>(SqlServerRepository.GetElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamQueryFirstOrDefault))), Times.Once);
    }

    [Theory(DisplayName = "Deve listar entries")]
    [MemberData(nameof(ScenariosListEntries))]
    public async Task ShouldListEntries(string instanceId, Entry[] items, Entry[] expectedResult)
    {
        // arrange
        var dataFetch = 100;
        _sqlServiceMock.Setup(lnq => lnq.QueryListAsync<Entry>(It.IsAny<string>(), It.IsAny<object?>())).ReturnsAsync(items);

        var expectedParamQueryListAsync = new
        {
            instanceId = instanceId,
            dataFetchThreshold = dataFetch
        };

        // act
        var result = await _repository.GetEntriesAsync(instanceId, dataFetch);

        // assert
        result.Should().BeEquivalentTo(expectedResult);

        _sqlServiceMock.Verify(lnq => lnq.QueryListAsync<Entry>(SqlServerRepository.ListElements, It.Is<object?>(t => t.BeEquivalentTo(expectedParamQueryListAsync))), Times.Once);
    }

    [Theory(DisplayName = "Deve validar se contem item")]
    [MemberData(nameof(ScenariosContainsItem))]
    public async Task ShouldValidateContainsItem(string instanceId, string idempotencyKey, bool containsInCircularBuffer, bool containsInSqlService, bool expectedResult, int countContainsMemoryCircularBuffer, int countContainsSqlService)
    {
        // arrange
        _inMemoryCircularBufferRepositoryMock.Setup(lnq => lnq.ContainsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(containsInCircularBuffer);
        _sqlServiceMock.Setup(lnq => lnq.ExistAsync(It.IsAny<string>(), It.IsAny<object?>())).ReturnsAsync(containsInSqlService);

        var expectedParamExistAsync = new
        {
            instanceId = instanceId,
            idempotencyKey = idempotencyKey
        };

        // act
        var result = await _repository.ContainsAsync(instanceId, idempotencyKey);

        // assert
        result.Should().Be(expectedResult);
        _inMemoryCircularBufferRepositoryMock.Verify(lnq => lnq.ContainsAsync(instanceId, idempotencyKey), Times.Exactly(countContainsMemoryCircularBuffer));
        _sqlServiceMock.Verify(lnq => lnq.ExistAsync(SqlServerRepository.ContainsElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamExistAsync))), Times.Exactly(countContainsSqlService));
    }

    [Fact(DisplayName = "Deve adicionar entry")]
    public async Task ShouldAddIEntry()
    {
        // arrange
        var data = new Entry("NewInstanceId", "NewIdempotencyKey", RepositoryEntryState.None, DateTime.Now);

        _sqlServiceMock.Setup(lnq => lnq.ExistAsync(It.IsAny<string>(), It.IsAny<object?>())).ReturnsAsync(false);

        var expectedParamExistAsync = new
        {
            instanceId = data.InstanceId,
            idempotencyKey = data.IdempotencyKey
        };

        var expectedParamInsertAsync = new
        {
            instanceId = data.InstanceId,
            idempotencyKey = data.IdempotencyKey,
            state = data.State.ToString(),
            timestamp = data.Timestamp
        };

        // act
        await _repository.AddOrUpdateAsync(data);

        // assert
        _inMemoryCircularBufferRepositoryMock.Verify(lnq => lnq.AddOrUpdateAsync(It.Is<Entry>(t => t.BeEquivalentTo(data))), Times.Once);
        _sqlServiceMock.Verify(lnq => lnq.ExistAsync(SqlServerRepository.ContainsElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamExistAsync))), Times.Once);
        _sqlServiceMock.Verify(lnq => lnq.UpdateAsync(It.IsAny<string>(), It.IsAny<object?>()), Times.Never);
        _sqlServiceMock.Verify(lnq => lnq.InsertAsync(SqlServerRepository.InsertElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamInsertAsync))), Times.Once);
    }

    [Fact(DisplayName = "Deve atualizar entry")]
    public async Task ShouldUpdateEntry()
    {
        // arrange
        var data = new Entry("NewInstanceId", "NewIdempotencyKey", RepositoryEntryState.None, DateTime.Now);

        _sqlServiceMock.Setup(lnq => lnq.ExistAsync(It.IsAny<string>(), It.IsAny<object?>())).ReturnsAsync(true);

        var expectedParamExistAsync = new
        {
            instanceId = data.InstanceId,
            idempotencyKey = data.IdempotencyKey
        };

        var expectedParamUpdateAsync = new
        {
            instanceId = data.InstanceId,
            idempotencyKey = data.IdempotencyKey,
            state = data.State.ToString(),
            timestamp = data.Timestamp
        };

        // act
        await _repository.AddOrUpdateAsync(data);

        // assert
        _inMemoryCircularBufferRepositoryMock.Verify(lnq => lnq.AddOrUpdateAsync(It.Is<Entry>(t => t.BeEquivalentTo(data))), Times.Once);
        _sqlServiceMock.Verify(lnq => lnq.ExistAsync(SqlServerRepository.ContainsElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamExistAsync))), Times.Once);
        _sqlServiceMock.Verify(lnq => lnq.UpdateAsync(SqlServerRepository.UpdateElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamUpdateAsync))), Times.Once);
        _sqlServiceMock.Verify(lnq => lnq.InsertAsync(It.IsAny<string>(), It.IsAny<object?>()), Times.Never);
    }

    [Fact(DisplayName = "Deve remover entry")]
    public async Task ShouldRemoveEntry()
    {
        // arrange
        var instanceId = Instance1Item1.InstanceId;
        var idempotencyKey = Instance1Item1.IdempotencyKey;

        var expectedParamDeleteAsync = new
        {
            InstanceId = instanceId,
            IdempotencyKey = idempotencyKey
        };

        // act
        await _repository.RemoveAsync(instanceId, idempotencyKey);

        // assert
        _inMemoryCircularBufferRepositoryMock.Verify(lnq => lnq.RemoveAsync(instanceId, idempotencyKey), Times.Once);
        _sqlServiceMock.Verify(lnq => lnq.DeleteAsync(SqlServerRepository.DeleteElement, It.Is<object?>(t => t.BeEquivalentTo(expectedParamDeleteAsync))), Times.Once);
    }

    public static IEnumerable<object[]> ScenariosGetEntry => new List<object[]>
    {
        new object[] {Instance1Item1.InstanceId, Instance1Item1.IdempotencyKey, Instance1Item1, Instance1Item1},
        new object[] {Instance1Item1.InstanceId, Instance1Item1.IdempotencyKey, null!, Entry.Empty}
    };

    public static IEnumerable<object[]> ScenariosListEntries => new List<object[]>
    {
        new object[] {InstanceId1, AllItems.Where(lnq => lnq.InstanceId == InstanceId1), AllItems.Where(lnq => lnq.InstanceId == InstanceId1)},
        new object[] {InstanceId2, AllItems.Where(lnq => lnq.InstanceId == InstanceId2), AllItems.Where(lnq => lnq.InstanceId == InstanceId2)},
        new object[] {InstanceId3, AllItems.Where(lnq => lnq.InstanceId == InstanceId3), AllItems.Where(lnq => lnq.InstanceId == InstanceId3)}
    };

    public static IEnumerable<object[]> ScenariosContainsItem => new List<object[]>
    {
        new object[] {InstanceId1, Instance1Item1.IdempotencyKey, true, true, true, 1, 0},
        new object[] {InstanceId1, Instance1Item1.IdempotencyKey, true, false, true, 1, 0},
        new object[] {InstanceId1, Instance1Item1.IdempotencyKey, false, true, true, 1, 1},
        new object[] {InstanceId1, Instance1Item1.IdempotencyKey, false, false, false, 1, 1}
    };
}