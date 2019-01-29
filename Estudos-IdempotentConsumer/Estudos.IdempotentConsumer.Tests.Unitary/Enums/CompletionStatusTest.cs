using System;
using System.Linq;
using Estudos.IdempotentConsumer.Enums;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Enums;

public class CompletionStatusTest
{
    [Fact(DisplayName = "Deve validar nomes dos enums")]
    public void ShouldValidateEnumNames()
    {
        // arrange
        var expectedResult = new[] {"None", "Ignored", "Consumed"};

        // act
        var names = Enum.GetNames<CompletionStatus>();

        // assert
        names.Should().BeEquivalentTo(expectedResult);
    }

    [Fact(DisplayName = "Deve validar valores dos enums")]
    public void ShouldValidateEnumValues()
    {
        // arrange
        var expectedResult = new[] {CompletionStatus.None, CompletionStatus.Ignored, CompletionStatus.Consumed};

        // act
        var values = Enum.GetValues<CompletionStatus>();

        // assert
        values.Should().BeEquivalentTo(expectedResult);
        values.Select(lnq => (int) lnq).Should().BeEquivalentTo(new[] {0, 1, 2});
    }
}