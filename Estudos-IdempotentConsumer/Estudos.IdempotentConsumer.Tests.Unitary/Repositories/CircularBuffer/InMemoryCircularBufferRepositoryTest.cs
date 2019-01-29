using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Options;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Repositories.CircularBuffer;

public class InMemoryCircularBufferRepositoryTest
{
    private const string InstanceId1 = "InstanceId-1";
    private const string InstanceId2 = "InstanceId-2";
    private const string InstanceId3 = "InstanceId-3";

    private readonly int _maxItemsBuffer;
    private readonly ConcurrentDictionary<string, EntryCircularBufferDt> _buffer;
    private readonly IInMemoryCircularBufferRepository _inMemoryCircularBufferRepository;

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

    public InMemoryCircularBufferRepositoryTest()
    {
        _maxItemsBuffer = 10;
        var options = new OptionsWrapper<InMemoryCircularBufferRepositoryOptions>(
            new InMemoryCircularBufferRepositoryOptions
            {
                MaxItemsBuffer = _maxItemsBuffer
            });

        _inMemoryCircularBufferRepository = new InMemoryCircularBufferRepository(options);
        _buffer = (ConcurrentDictionary<string, EntryCircularBufferDt>) _inMemoryCircularBufferRepository.GetType().GetField("_buffer", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(_inMemoryCircularBufferRepository)!;
    }

    [Theory(DisplayName = "Deve validar tamanho buffer")]
    [MemberData(nameof(ScenarioValidateMaxItemsBuffer))]
    public void ShouldValidateMaxItemsBuffer(InMemoryCircularBufferRepositoryOptions options, int expectedResult)
    {
        // arrange
        var optionsWrapper = new OptionsWrapper<InMemoryCircularBufferRepositoryOptions>(options);

        var inMemoryCircularBufferRepository = new InMemoryCircularBufferRepository(optionsWrapper);

        // act
        var maxItemsBuffer = inMemoryCircularBufferRepository.MaxItemsBuffer;

        // assert
        maxItemsBuffer.Should().Be(expectedResult);
    }

    [Theory(DisplayName = "Deve retornar Entry")]
    [MemberData(nameof(ScenariosGetEntry))]
    public async Task ShouldGetEntry(string instanceId, string idempotencyKey, Entry expectedResult)
    {
        // arrange
        AddDefaultItemsInBuffer();

        // act
        var result = await _inMemoryCircularBufferRepository.GetEntryAsync(instanceId, idempotencyKey);

        // assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = "Deve retornar Entries de uma instância")]
    [MemberData(nameof(ScenariosGetEntriesFromInstanceId))]
    public async Task ShouldGetEntriesOfInstanceId(string instanceId, IEnumerable<Entry> expectedResult)
    {
        // arrange
        AddDefaultItemsInBuffer();

        // act
        var entries = await _inMemoryCircularBufferRepository.GetEntriesAsync(instanceId, _maxItemsBuffer);

        // assert
        entries.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = "Deve validar se contem Entry no buffer")]
    [MemberData(nameof(ScenariosContainsEntryInBuffer))]
    public async Task ShouldValidateContainsEntryInBuffer(string instanceId, string idempotencyKey, bool expectedResult)
    {
        // arrange
        AddDefaultItemsInBuffer();

        // act
        var result = await _inMemoryCircularBufferRepository.ContainsAsync(instanceId, idempotencyKey);

        // assert
        result.Should().Be(expectedResult);
    }

    [Theory(DisplayName = "Deve validar adição ou atualização de um item no buffer")]
    [MemberData(nameof(ScenariosAddOrUpdateItemInBuffer))]
    public async Task ShouldValidateAddOrUpdateItemInBuffer(Entry[] data, Entry expectedResult, int expectedBufferSize)
    {
        // arrange - act
        foreach (var entry in data)
            await _inMemoryCircularBufferRepository.AddOrUpdateAsync(entry);


        var result = await _inMemoryCircularBufferRepository.GetEntryAsync(expectedResult.InstanceId, expectedResult.IdempotencyKey);

        var bufferSize = (await _inMemoryCircularBufferRepository.GetEntriesAsync(expectedResult.InstanceId, _maxItemsBuffer)).Count();

        // assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.Excluding(prop => prop.Timestamp));
        result.Timestamp.Date.Should().Be(expectedResult.Timestamp.Date);

        bufferSize.Should().Be(expectedBufferSize);
    }

    [Theory(DisplayName = "Deve remover item do buffer")]
    [MemberData(nameof(ScenariosDeleteItemFromBuffer))]
    public async Task ShouldRemoveItemFromBuffer(Entry[] data, string instanceId, string idempotencyKey, int expectedSizedCircularBufferDt, int expectedBufferSize)
    {
        // arrange
        foreach (var entry in data)
            await _inMemoryCircularBufferRepository.AddOrUpdateAsync(entry);

        // act
        await _inMemoryCircularBufferRepository.RemoveAsync(instanceId, idempotencyKey);

        var sizeCircularBufferDt = (await _inMemoryCircularBufferRepository.GetEntriesAsync(instanceId, _maxItemsBuffer)).Count();
        var bufferSize = _buffer.Count;

        // assert
        sizeCircularBufferDt.Should().Be(expectedSizedCircularBufferDt);
        bufferSize.Should().Be(expectedBufferSize);
    }

    [Theory(DisplayName = "Deve adicionar entries")]
    [MemberData(nameof(ScenariosSetEntries))]
    public async Task ShouldSetEntries(string instanceId, Entry[] entries, Entry[] newEntries, int expectedSizeBuffer, int expectedSizedCircularBufferDt)
    {
        // arrange
        foreach (var entry in entries)
            await _inMemoryCircularBufferRepository.AddOrUpdateAsync(entry);

        // act
        _inMemoryCircularBufferRepository.SetEntries(instanceId, newEntries);

        var sizedCircularBufferDt = (await _inMemoryCircularBufferRepository.GetEntriesAsync(instanceId, _maxItemsBuffer)).Count();

        // assert
        _buffer.Count.Should().Be(expectedSizeBuffer);
        sizedCircularBufferDt.Should().Be(expectedSizedCircularBufferDt);
    }

    private void AddDefaultItemsInBuffer()
    {
        foreach (var item in AllItems)
        {
            if (!_buffer.TryGetValue(item.InstanceId, out var circularBufferDt))
            {
                circularBufferDt = new EntryCircularBufferDt(_maxItemsBuffer);
                _buffer.TryAdd(item.InstanceId, circularBufferDt);
            }

            var data = new Entry(item.InstanceId, item.IdempotencyKey, RepositoryEntryState.None, DateTime.Now);
            circularBufferDt.AddOrUpdate(data);
        }
    }

    public static IEnumerable<object[]> ScenarioValidateMaxItemsBuffer => new List<object[]>
    {
        new object[] {null!, 100},
        new object[] {new InMemoryCircularBufferRepositoryOptions {MaxItemsBuffer = 5}, 5},
        new object[] {new InMemoryCircularBufferRepositoryOptions {MaxItemsBuffer = 0}, 100}
    };

    public static IEnumerable<object[]> ScenariosGetEntry => new List<object[]>
    {
        new object[] {"None", "None", Entry.Empty},
        new object[] {InstanceId1, "None", Entry.Empty},
        new object[] {"None", Instance1Item2.IdempotencyKey, Entry.Empty},
        new object[] {InstanceId1, Instance1Item2.IdempotencyKey, Instance1Item2},
        new object[] {InstanceId2, Instance2Item2.IdempotencyKey, Instance2Item2},
        new object[] {InstanceId3, Instance3Item2.IdempotencyKey, Instance3Item2}
    };

    public static IEnumerable<object[]> ScenariosGetEntriesFromInstanceId => new List<object[]>
    {
        new object[] {"None", Array.Empty<Entry>()},
        new object[] {Instance1Item1.InstanceId, AllItems.Where(lnq => lnq.InstanceId == InstanceId1)},
        new object[] {Instance2Item1.InstanceId, AllItems.Where(lnq => lnq.InstanceId == InstanceId2)},
        new object[] {Instance3Item1.InstanceId, AllItems.Where(lnq => lnq.InstanceId == InstanceId3)}
    };

    public static IEnumerable<object[]> ScenariosContainsEntryInBuffer => new List<object[]>
    {
        new object[] {"None", "None", false},
        new object[] {InstanceId1, "None", false},
        new object[] {"None", Instance1Item3.IdempotencyKey, false},
        new object[] {InstanceId1, Instance1Item3.IdempotencyKey, true},
        new object[] {InstanceId2, Instance2Item3.IdempotencyKey, true},
        new object[] {InstanceId3, Instance3Item3.IdempotencyKey, true}
    };

    public static IEnumerable<object[]> ScenariosAddOrUpdateItemInBuffer => new List<object[]>
    {
        new object[] {new[] {Instance1Item1}, Instance1Item1, 1},
        new object[] {new[] {Instance1Item1, Instance1Item2, Instance1Item3, Instance2Item1}, Instance2Item1, 1},
        new object[] {new[] {Instance1Item1, Instance1Item2, Instance1Item3, Instance2Item1}, Instance2Item1, 1},
        new object[] {new[] {Instance1Item1, Instance1Item2, Instance1Item3, Instance2Item1, Instance2Item2, Instance2Item3}, new Entry(Instance2Item1.InstanceId, Instance2Item1.IdempotencyKey, RepositoryEntryState.Processing, DateTime.Now), 3}
    };

    public static IEnumerable<object[]> ScenariosDeleteItemFromBuffer => new List<object[]>
    {
        new object[] {new[] {Instance1Item1}, Instance1Item1.InstanceId, Instance1Item1.IdempotencyKey, 0, 0},
        new object[] {new[] {Instance1Item1}, Instance2Item1.InstanceId, Instance1Item1.IdempotencyKey, 0, 1},
        new object[] {new[] {Instance1Item1, Instance1Item2, Instance1Item3}, Instance1Item1.InstanceId, Instance1Item1.IdempotencyKey, 2, 1},
        new object[] {AllItems, Instance1Item1.InstanceId, Instance1Item1.IdempotencyKey, 2, 3}
    };

    public static IEnumerable<object[]> ScenariosSetEntries => new List<object[]>
    {
        new object[] {InstanceId1, Array.Empty<Entry>(), new[] {Instance1Item1, Instance1Item2, Instance1Item3}, 1, 3},
        new object[] {InstanceId1, new[] {Instance1Item1}, new[] {Instance1Item1, Instance1Item2, Instance1Item3}, 1, 1}
    };
}