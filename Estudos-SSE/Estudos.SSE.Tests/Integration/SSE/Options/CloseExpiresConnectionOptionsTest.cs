using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Integration.Collections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Tests.Integration.SSE.Options
{
    [Collection(nameof(SseCollectionTest))]
    public class CloseExpiresConnectionOptionsTest : BaseTest
    {
        public CloseExpiresConnectionOptionsTest()
        {
            ConfigureServices += (_, collection) => { collection.Configure<CloseExpiresConnectionOptions>(opt => { opt.CloseConnectionsInSecondsInterval = 10; }); };
        }

        [Fact(DisplayName = "Deve validar objeto CloseExpiresConnectionOptions")]
        public void ShouldValidateObjectCloseExpiresConnectionOptions()
        {
            // arrange - act
            var result = ServiceProvider.GetRequiredService<IOptions<CloseExpiresConnectionOptions>>().Value;

            // assert
            result.CloseConnectionsInSecondsInterval.Should().Be(10);
        }
    }
}