

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Options
{
    public class SseServiceOptionsTest
    {
        [Fact(DisplayName = "Deve validar objeto SseServiceOptions")]
        public void ShouldValidateObjectSseServiceOptions()
        {
            // arrange 
            var connectedCount = 0;
            var disconnectedCount = 0;

            // act
            var result = new SseServiceOptions
            {
                OnClientConnected = (_, _) => { connectedCount++; },
                OnClientDisconnected = (_, _) => { disconnectedCount++; }
            };

            result.OnClientConnected.Invoke(default!, default!);
            result.OnClientDisconnected.Invoke(default!, default!);

            // assert
            connectedCount.Should().Be(1);
            disconnectedCount.Should().Be(1);
        }

        [Fact(DisplayName = "Deve validar propriedades do objeto SseServiceOptions")]
        public void ShouldValidateObjectSseServiceOptionsProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "OnClientConnected", Type = typeof(Action<IServiceProvider, SseClient>)},
                new() {Name = "OnClientDisconnected", Type = typeof(Action<IServiceProvider, SseClient>)}
            };

            // act - assert
            typeof(SseServiceOptions).ValidateProperties(expectedProperties);
        }
    }
}