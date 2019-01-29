

// ReSharper disable InconsistentNaming

using Estudos.SSE.Extensions;
using Estudos.SSE.Tests.Integration.Collections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Tests.Integration.SSE.SSE.Extensions
{
    [Collection(nameof(SseCollectionTest))]
    public class SseOptionsTest : BaseTest
    {
        private int _countOnPrepareAccept;
        private int _countOnClientConnected;
        private int _countOnClientDisconnected;

        public SseOptionsTest()
        {
            ConfigureServices += (_, collection) =>
            {
                collection.Configure<SseOptions>(
                    opt =>
                    {
                        opt.CloseConnectionsInSecondsInterval = 10;
                        opt.MaxTimeCacheInMinutes = 5;
                        opt.OnPrepareAccept = _ => { _countOnPrepareAccept++; };
                        opt.OnClientConnected = (_, _) => { _countOnClientConnected++; };
                        opt.OnClientDisconnected = (_, _) => { _countOnClientDisconnected++; };
                    });
            };
        }

        [Fact(DisplayName = "Deve validar objeto SseOptions")]
        public void ShouldValidateObjectSseOptions()
        {
            // arrange
            var result = ServiceProvider.GetRequiredService<IOptions<SseOptions>>().Value!;

            // act
            result.OnPrepareAccept!(null!);
            result.OnClientConnected!(null!, null!);
            result.OnClientDisconnected!(null!, null!);

            // assert
            result.CloseConnectionsInSecondsInterval.Should().Be(10);
            result.MaxTimeCacheInMinutes.Should().Be(5);
            result.OnPrepareAccept.Should().NotBeNull();
            result.OnClientConnected.Should().NotBeNull();
            result.OnClientDisconnected.Should().NotBeNull();

            _countOnPrepareAccept.Should().Be(1);
            _countOnClientConnected.Should().Be(1);
            _countOnClientDisconnected.Should().Be(1);
        }
    }
}