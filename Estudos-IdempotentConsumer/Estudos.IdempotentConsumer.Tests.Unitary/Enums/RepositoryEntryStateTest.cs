using System;
using System.Linq;
using Estudos.IdempotentConsumer.Enums;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Enums;

public class RepositoryEntryStateTest
{
    [Fact(DisplayName = "Deve validar nomes dos enums")]
    public void ShouldValidateEnumNames()
    {
        // arrange
        var expectedResult = new[] {"None", "Reserved", "Processing", "Committed"};

        // act
        var names = Enum.GetNames<RepositoryEntryState>();

        // assert
        names.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Deve validar valores dos enums")]
    public void ShouldValidateEnumValues()
    {
        // arrange
        var expectedResult = new[] {RepositoryEntryState.None, RepositoryEntryState.Reserved, RepositoryEntryState.Processing, RepositoryEntryState.Committed};

        // act
        var values = Enum.GetValues<RepositoryEntryState>();

        // assert
        values.Should().BeEquivalentTo(expectedResult);
        values.Select(lnq => (int) lnq).Should().BeEquivalentTo(new[] {0, 1, 2, 3});
    }
}