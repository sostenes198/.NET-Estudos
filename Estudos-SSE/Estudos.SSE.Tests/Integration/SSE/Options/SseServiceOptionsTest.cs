

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Integration.Collections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Tests.Integration.SSE.Options
{
    [Collection(nameof(SseCollectionTest))]
    public class SseServiceOptionsTest : BaseTest
    {
        public SseServiceOptionsTest()
        {
            ConfigureServices += (_, collection) =>
            {
                collection.AddSingleton<OnClientConnected>();
                collection.AddSingleton<OnClientDisconnected>();
                collection.Configure<SseServiceOptions>(
                    options =>
                    {
                        options.OnClientConnected += (provider, _) =>
                        {
                            provider.GetRequiredService<OnClientConnected>().Count++;
                        };
                        options.OnClientDisconnected += (provider, _) =>
                        {
                            provider.GetRequiredService<OnClientDisconnected>().Count++;
                        };
                    });
            };
        }

        public class OnClientConnected
        {
            public int Count { get; set; }
        }

        public class OnClientDisconnected
        {
            public int Count { get; set; }
        }

        [Fact(DisplayName = "Deve validar SseServiceOptions")]
        public void ShouldValidateObjectSseServiceOptions()
        {
            // arrange
            var options = ServiceProvider.GetRequiredService<IOptions<SseServiceOptions>>().Value;

            // act
            options.OnClientConnected!.Invoke(ServiceProvider, default!);
            options.OnClientDisconnected!.Invoke(ServiceProvider, default!);


            var resultOnClientConnected = ServiceProvider.GetRequiredService<OnClientConnected>().Count;
            var resultOnClientDisconnected = ServiceProvider.GetRequiredService<OnClientDisconnected>().Count;

            // assert
            resultOnClientConnected.Should().Be(1);
            resultOnClientDisconnected.Should().Be(1);
        }
    }
}