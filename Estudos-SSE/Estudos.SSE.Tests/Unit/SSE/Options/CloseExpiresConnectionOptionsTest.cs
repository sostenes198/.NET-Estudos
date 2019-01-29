using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Options
{
    public class CloseExpiresConnectionOptionsTest
    {
        [Theory(DisplayName = "Deve validar objeto CloseExpiresConnectionOptions")]
        [InlineData(0, 60)]
        [InlineData(10, 10)]
        public void ShouldValidateObjectCloseExpiresConnectionOptions(int closeConnectionsInSecondsInterval, int closeConnectionsInSecondsIntervalExpected)
        {
            // arrange - act
            var result = new CloseExpiresConnectionOptions
            {
                CloseConnectionsInSecondsInterval = closeConnectionsInSecondsInterval
            };
            
            // assert
            result.CloseConnectionsInSecondsInterval.Should().Be(closeConnectionsInSecondsIntervalExpected);
        }

        [Fact(DisplayName = "Deve validar propriedades do objeto CloseExpiresConnectionOptions")]
        public void ShouldValidateObjectCloseExpiresConnectionOptionsProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "CloseConnectionsInSecondsInterval", Type = typeof(int)}
            };

            // act - assert
            typeof(CloseExpiresConnectionOptions).ValidateProperties(expectedProperties);
        }
    }
}