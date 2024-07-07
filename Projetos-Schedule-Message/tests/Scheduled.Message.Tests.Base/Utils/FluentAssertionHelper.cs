using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;

namespace Scheduled.Message.Tests.Base.Utils;

public static class FluentAssertionHelper
{
    public static bool Comparer(this string result, string expectedResult)
    {
        result.Should().BeEquivalentTo(expectedResult);

        return true;
    }

    public static bool Comparer<TExpectedResult>(this ObjectAssertions objectAssertions, TExpectedResult expectedResult)
    {
        objectAssertions.BeEquivalentTo(expectedResult);

        return true;
    }

    public static bool Comparer<TExpectedResult>(
        this ObjectAssertions objectAssertions,
        TExpectedResult expectedResult,
        Func<EquivalencyAssertionOptions<TExpectedResult>, EquivalencyAssertionOptions<TExpectedResult>> config)
    {
        objectAssertions.BeEquivalentTo(expectedResult, config);

        return true;
    }
}