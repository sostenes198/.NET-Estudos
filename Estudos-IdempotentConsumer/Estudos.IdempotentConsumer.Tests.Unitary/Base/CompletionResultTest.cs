using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Enums;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Base;

public class CompletionResultTest
{
    [Fact(DisplayName = "Deve criar objeto CompletionResult")]
    public void ShouldCreateCompletionResultObject()
    {
        // arrange - act 
        var result = new CompletionResult<Result>(CompletionStatus.Consumed, new Result{Id = 10, Name = "SS"});

        // assert
        result.Result!.Id.Should().Be(10);
        result.Result.Name.Should().BeEquivalentTo("SS");
        result.CompletionStatus.Should().Be(CompletionStatus.Consumed);
    }

    private class Result
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}