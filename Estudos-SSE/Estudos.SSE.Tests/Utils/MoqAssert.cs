using FluentAssertions;
using Moq;

namespace Estudos.SSE.Tests.Utils;

public static class MoqAssert
{
    public static T Assert<T>(T result)
        where T : class
    {
        bool FluentAssertion(T matchParam)
        {
            matchParam.Should().BeEquivalentTo(result);

            return true;
        }

        return Match.Create<T>(FluentAssertion);
    }
}