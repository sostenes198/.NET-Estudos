using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Repositories.CircularBuffer;

public class EntryCircularBufferDtTest
{
    private static readonly Entry Item1 = new("InstanceId", "Key-1", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Item2 = new("InstanceId", "Key-2", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry Item3 = new("InstanceId", "Key-3", RepositoryEntryState.None, DateTime.Now);
    private static readonly Entry DefaultEntry = new("InstanceId", "IdempotencyKey", RepositoryEntryState.Processing, DateTime.Now);

    [Theory(DisplayName = "Deve criar objeto")]
    [InlineData(0)]
    [InlineData(3)]
    public void ShouldCreateObject(int capacity)
    {
        // arrange - act
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new EntryCircularBufferDt(capacity);

        // assert
        act.Should().NotThrow();
    }

    [Theory(DisplayName = "Deve criar objeto com items")]
    [MemberData(nameof(ScenariosCreateObjectWithItem))]
    public void ShouldCreateObjectWithItems(int capacity, Entry[] items)
    {
        // arrange - act
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new EntryCircularBufferDt(capacity, items);

        // assert
        act.Should().NotThrow();
    }

    [Fact(DisplayName = "Deve lançar exceção quando capacidade for menor que 0")]
    public void ShouldThrownExceptionWhenCapacityLowerThan0()
    {
        // arrange - act
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new EntryCircularBufferDt(-1);

        // assert
        act.Should().ThrowExactly<ArgumentException>().WithMessage("Circular buffer cannot have negative capacity. (Parameter 'capacity')");
    }

    [Fact(DisplayName = "Deve lançar exceção quando items for nullo")]
    public void ShouldThrownExceptionWhenNullItems()
    {
        // arrange - act
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new EntryCircularBufferDt(1, null!);

        // assert
        act.Should().ThrowExactly<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'items')");
    }

    [Fact(DisplayName = "Deve lançar exceção quando tamanho dos items informados for maior do que a capacidade informada")]
    public void ShouldThrownExceptionWhenItemsLenghtIsGreaterThanCapacity()
    {
        // arrange - act
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new EntryCircularBufferDt(1, new[] {DefaultEntry, DefaultEntry});

        // assert
        act.Should().ThrowExactly<ArgumentException>().WithMessage("Too many items to fit circular buffer. (Parameter 'items')");
    }

    [Fact(DisplayName = "Deve validar Enumerator")]
    public void ShouldValidateEnumerator()
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(3, new[] {Item1, Item2, Item3});

        // act - assert
        using var enumerator = entryCircularBufferDt.GetEnumerator();
        var next = enumerator.MoveNext();
        next.Should().BeTrue();
        enumerator.Current.Should().BeEquivalentTo(Item1);

        next = enumerator.MoveNext();
        next.Should().BeTrue();
        enumerator.Current.Should().BeEquivalentTo(Item2);

        next = enumerator.MoveNext();
        next.Should().BeTrue();
        enumerator.Current.Should().BeEquivalentTo(Item3);

        next = enumerator.MoveNext();
        next.Should().BeFalse();
    }

    [Theory(DisplayName = "Deve validar EntryCircularBufferDt vazio")]
    [MemberData(nameof(ScenariosEmptyEntryCircularBufferDt))]
    public void ShouldValidateEmptyEntryCircularBufferDt(int capacity, Entry[] items, bool expectedResult)
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(capacity, items);

        // act - assert
        entryCircularBufferDt.IsEmpty.Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Deve validar tamanho EntryCircularBufferDt")]
    public void ShouldValidateEntryCircularBufferDt()
    {
        // arrange - act
        var entryCircularBufferDt = new EntryCircularBufferDt(10);

        // assert
        entryCircularBufferDt.Lenght.Should().Be(10);
    }

    [Theory(DisplayName = "Deve obter Entry")]
    [MemberData(nameof(ScenariosGetEntryFromEntryCircularBufferDt))]
    public void ShouldGetEntry(int capacity, Entry[] items, Entry expectedResult)
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(capacity, items);

        // act
        var entry = entryCircularBufferDt.GetEntry(Item2.InstanceId, Item2.IdempotencyKey);

        // assert
        entry.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = "Deve obter todos Entry do EntryCircularBufferDt")]
    [MemberData(nameof(ScenariosGetEntriesFromEntryCircularBufferDt))]
    public void ShouldGetAllEntriesFromEntryCircularBufferDt(int capacity, Entry[] items, Entry[] expectedResult)
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(capacity, items);

        // act
        var result = entryCircularBufferDt.GetEntries();

        // assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Theory(DisplayName = "Deve validar contains Entry no EntryCircularBufferDt")]
    [MemberData(nameof(ScenariosContainsEntryInEntryCircularBufferDt))]
    public void ShouldValidateContainsEntryInEntryCircularBufferDt(int capacity, Entry[] items, bool expectedResult)
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(capacity, items);

        // act
        var result = entryCircularBufferDt.Contains(Item2.InstanceId, Item2.IdempotencyKey);

        // assert
        result.Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Deve adicionar ou atualizar Entry no EntryCircularBufferDt com capacidade vazia")]
    public void ShouldAddOrUpdateEntryInEntryCircularBufferDtWithEmptyCapacity()
    {
        // arrange
        var instanceId = "newInstanceId";
        var idempotencyKey = "newIdempotencyKey";
        var entryCircularBufferDt = new EntryCircularBufferDt(0);

        // act
        entryCircularBufferDt.AddOrUpdate(new Entry(instanceId, idempotencyKey, RepositoryEntryState.None, DateTime.Now));

        var result = entryCircularBufferDt.GetEntry(instanceId, idempotencyKey);

        // assert
        result.Should().BeEquivalentTo(Entry.Empty);

        var entriesBuffer = entryCircularBufferDt.GetEntries();
        entriesBuffer.Should().BeEmpty();
    }

    [Theory(DisplayName = "Deve adicionar ou atualizar Entry no EntryCircularBufferDt")]
    [MemberData(nameof(ScenariosAddOrUpdateEntryInEntryCircularBufferDt))]
    public void ShouldValidateEntryInEntryCircularBufferDt(int capacity, Entry[] items, string newInstanceId, string newIdempotencyKey, RepositoryEntryState state, int entriesResultSize)
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(capacity, items);

        // act
        entryCircularBufferDt.AddOrUpdate(new Entry(newInstanceId, newIdempotencyKey, state, DateTime.Now));

        var result = entryCircularBufferDt.GetEntry(newInstanceId, newIdempotencyKey);

        // assert
        result.Should().NotBeEquivalentTo(Entry.Empty);
        result.InstanceId.Should().BeEquivalentTo(newInstanceId);
        result.IdempotencyKey.Should().BeEquivalentTo(newIdempotencyKey);
        result.State.Should().Be(state);

        var entriesBuffer = entryCircularBufferDt.GetEntries();
        entriesBuffer.Count().Should().Be(entriesResultSize);
    }

    [Theory(DisplayName = "Deve remover Entry do EntryCircularBufferDt")]
    [MemberData(nameof(ScenariosRemoveEntryFromEntryCircularBufferDt))]
    public void ShouldRemoveEntryFromEntryCircularBufferDt(int capacity, Entry[] items, string instanceId, string idempotencyKey, int sizeEntriesExpected)
    {
        // arrange
        var entryCircularBufferDt = new EntryCircularBufferDt(capacity, items);

        // act
        entryCircularBufferDt.Remove(instanceId, idempotencyKey);

        var entriesBuffer = entryCircularBufferDt.GetEntries();

        // assert
        entriesBuffer.Count().Should().Be(sizeEntriesExpected);
    }

    [Fact(DisplayName = "Deve validar Circular Buffer")]
    public void ShouldValidateCircularBuffer()
    {
        // arrange
        var maxCapacity = 3;

        var item4 = new Entry("InstanceId", "Key-4", RepositoryEntryState.None, DateTime.Now);
        var item5 = new Entry("InstanceId", "Key-5", RepositoryEntryState.None, DateTime.Now);
        var item6 = new Entry(Item3.InstanceId, Item3.IdempotencyKey, RepositoryEntryState.Processing, DateTime.Now);

        var entryCircularBufferDt = new EntryCircularBufferDt(maxCapacity, new[] {Item1});

        var buffer = (Entry?[]) entryCircularBufferDt.GetType().GetField("_buffer", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(entryCircularBufferDt)!;

        // act
        entryCircularBufferDt.AddOrUpdate(Item1);
        entryCircularBufferDt.AddOrUpdate(Item2);
        entryCircularBufferDt.AddOrUpdate(Item3);
        entryCircularBufferDt.AddOrUpdate(item4);
        entryCircularBufferDt.AddOrUpdate(item5);
        entryCircularBufferDt.AddOrUpdate(item6);

        // assert
        buffer[0].Should().BeEquivalentTo(item4);
        buffer[1].Should().BeEquivalentTo(item5);
        buffer[2].Should().BeEquivalentTo(item6);
    }

    public static IEnumerable<object[]> ScenariosCreateObjectWithItem => new List<object[]>
    {
        new object[] {0, new Entry[] { }},
        new object[] {3, new[] {DefaultEntry, DefaultEntry, DefaultEntry}},
        new object[] {5, new[] {DefaultEntry, DefaultEntry, DefaultEntry}}
    };

    public static IEnumerable<object[]> ScenariosEmptyEntryCircularBufferDt => new List<object[]>
    {
        new object[] {0, Array.Empty<Entry>(), true},
        new object[] {3, new[] {Entry.Empty, Entry.Empty, Entry.Empty}, true},
        new object[] {5, new[] {Entry.Empty, Entry.Empty, Entry.Empty}, true},
        new object[] {3, new[] {DefaultEntry, DefaultEntry}, false},
        new object[] {5, new[] {DefaultEntry, Entry.Empty, Entry.Empty, Entry.Empty}, false}
    };

    public static IEnumerable<object[]> ScenariosGetEntryFromEntryCircularBufferDt = new List<object[]>
    {
        new object[] {0, Array.Empty<Entry>(), Entry.Empty},
        new object[] {10, Array.Empty<Entry>(), Entry.Empty},
        new object[] {2, new[] {Item1, Item3}, Entry.Empty},
        new object[] {10, new[] {Item1, Item3}, Entry.Empty},
        new object[] {3, new[] {Item1, Item2, Item3}, Item2},
        new object[] {10, new[] {Item1, Item2, Item3}, Item2}
    };

    public static IEnumerable<object[]> ScenariosGetEntriesFromEntryCircularBufferDt = new List<object[]>
    {
        new object[] {0, Array.Empty<Entry>(), Array.Empty<Entry>()},
        new object[] {3, new[] {Item1, Item2, Item3}, new[] {Item1, Item2, Item3}},
        new object[] {10, new[] {Item1, Item2, Item3}, new[] {Item1, Item2, Item3}}
    };

    public static IEnumerable<object[]> ScenariosContainsEntryInEntryCircularBufferDt => new List<object[]>
    {
        new object[] {0, Array.Empty<Entry>(), false},
        new object[] {10, Array.Empty<Entry>(), false},
        new object[] {2, new[] {Item1, Item3}, false},
        new object[] {10, new[] {Item1, Item3}, false},
        new object[] {3, new[] {Item1, Item2, Item3}, true},
        new object[] {10, new[] {Item1, Item2, Item3}, true}
    };

    public static IEnumerable<object[]> ScenariosAddOrUpdateEntryInEntryCircularBufferDt => new List<object[]>
    {
        new object[] {10, new Entry[] { }, "New-InstanceId", "New-Key", RepositoryEntryState.None, 1},
        new object[] {10, new[] {Item1, Item2, Item3}, "New-InstanceId", "New-Key", RepositoryEntryState.None, 4},
        new object[] {10, new[] {Item1, Item2, Item3, new Entry("New-InstanceId", "New-Key", RepositoryEntryState.Reserved, DateTime.Now)}, "New-InstanceId", "New-Key", RepositoryEntryState.Committed, 4}
    };

    public static IEnumerable<object[]> ScenariosRemoveEntryFromEntryCircularBufferDt => new List<object[]>
    {
        new object[] {0, Array.Empty<Entry>(), Item2.InstanceId, Item2.IdempotencyKey, 0},
        new object[] {10, new[] {Item1, Item3}, Item2.InstanceId, Item2.IdempotencyKey, 2},
        new object[] {10, new[] {Item1, Item2, Item3}, Item2.InstanceId, Item2.IdempotencyKey, 2},
        new object[] {10, new[] {Item1, Item2, Item3, DefaultEntry}, Item2.InstanceId, Item2.IdempotencyKey, 3}
    };
}