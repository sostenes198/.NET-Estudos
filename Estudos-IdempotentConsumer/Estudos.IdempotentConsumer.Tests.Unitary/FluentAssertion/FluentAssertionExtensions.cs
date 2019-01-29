using FluentAssertions;

namespace Estudos.IdempotentConsumer.Tests.Unitary.FluentAssertion;

public static class FluentAssertionExtensions
{
    public static bool BeEquivalentTo<T>(this T obj, T expectationResult)
    {
        obj.Should().BeEquivalentTo(expectationResult);
        return true;
    }
}