using FluentAssertions;
using FluentAssertions.Equivalency;
using Moq;

namespace Estudos.CleanArchitecture.Modular.BaseTests
{
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
    
        public static T Assert<T>(T result,  Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config)
            where T : class
        {
            bool FluentAssertion(T matchParam)
            {
                matchParam.Should().BeEquivalentTo(result, config);

                return true;
            }

            return Match.Create<T>(FluentAssertion);
        }
    }
}