using System;
using System.Collections.Generic;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Repositories.Base;

public class EntryTest
{
    private static readonly Entry DefaultEntry = new("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now);


    [Fact(DisplayName = "Deve criar objeto Entry")]
    public void ShouldCreateEntryObject()
    {
        // arrange - act
        var result = new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.Reserved, DateTime.Now);

        // assert
        ValidateObjectProperties(result, DateTime.Now.Date, "InstanceId", "IdempotencyKey", RepositoryEntryState.Reserved);
    }

    [Theory(DisplayName = "Deve validar se objeto existe")]
    [InlineData("InstanceId", "IdempotencyKey", true)]
    [InlineData("", "IdempotencyKey", false)]
    [InlineData("InstanceId", "", false)]
    [InlineData("", "", false)]
    public void ShouldValidateExistEntryObject(string instanceId, string idempotencyKey, bool expectedResult)
    {
        // arrange
        var entry = new Entry(instanceId, idempotencyKey, RepositoryEntryState.Processing, DateTime.Now);

        // act - assert
        entry.Exist().Should().Be(expectedResult);
    }

    [Theory(DisplayName = "Deve validar comparação do objeto")]
    [MemberData(nameof(ScenariosEntryEquals))]
    public void ShouldValidateEqualsEntryObject(Entry entry, object equalsObjectToCompare, bool expectedResult)
    {
        // arrange - act - assert
        entry.Equals(equalsObjectToCompare).Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Deve validar obtenção do hash code")]
    public void ShouldValidateGetHashCode()
    {
        // assert
        var entry = new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.Processing, DateTime.Now);

        // act
        var result = entry.GetHashCode();

        // assert
        result.Should().NotBe(0);
    }

    [Fact(DisplayName = "Deve criar objeto Entry empty")]
    public void ShouldCreateEmptyEntry()
    {
        // arrange - act
        var result = Entry.Empty;

        // assert
        ValidateObjectProperties(result, default, string.Empty, string.Empty, RepositoryEntryState.None);
    }

    [Theory(DisplayName = "Deve validar predicate")]
    [InlineData("InstanceId", "IdempotencyKey", true)]
    [InlineData("", "IdempotencyKey", false)]
    [InlineData("InstanceId", "", false)]
    [InlineData("false", "", false)]
    public void ShouldValidatePredicate(string instanceId, string idempotencyKey, bool expectedResult)
    {
        // arrange
        var entry = new Entry(instanceId, idempotencyKey, RepositoryEntryState.Processing, DateTime.Now);

        // act
        var predicate = Entry.Predicate("InstanceId", "IdempotencyKey");

        // assert
        predicate.Invoke(entry).Should().Be(expectedResult);
    }

    [Fact(DisplayName = "Deve validar predicate com entry nullo")]
    public void ShouldValidatePredicateWitNullPredicate()
    {
        // arrange - act
        var predicate = Entry.Predicate("InstanceId", "IdempotencyKey");

        // assert
        predicate.Invoke(null!).Should().BeFalse();
    }

    private static void ValidateObjectProperties(Entry entry, DateTime timestamp, string instanceId, string idempotencyKey, RepositoryEntryState state)
    {
        entry.Timestamp.Date.Should().Be(timestamp);
        entry.InstanceId.Should().BeEquivalentTo(instanceId);
        entry.IdempotencyKey.Should().BeEquivalentTo(idempotencyKey);
        entry.State.Should().Be(state);
    }

    public static IEnumerable<object[]> ScenariosEntryEquals => new List<object[]>
    {
        new object[] {DefaultEntry, DefaultEntry, true},
        new object[] {Entry.Empty, null!, false},
        new object[] {Entry.Empty, null!, false},
        new object[] {Entry.Empty, new(), false},
        new object[] {new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), true},
        new object[] {new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), new Entry("", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), false},
        new object[] {new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), new Entry("InstanceId", "", RepositoryEntryState.None, DateTime.Now), false},
        new object[] {new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), new Entry("", "", RepositoryEntryState.None, DateTime.Now), false},
        new object[] {new Entry("", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), false},
        new object[] {new Entry("InstanceId", "", RepositoryEntryState.None, DateTime.Now), new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), false},
        new object[] {new Entry("", "", RepositoryEntryState.None, DateTime.Now), new Entry("InstanceId", "IdempotencyKey", RepositoryEntryState.None, DateTime.Now), false}
    };
}