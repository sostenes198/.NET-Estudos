

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.Events;
using Estudos.SSE.Tests.Fixtures;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Tests.Unit.SSE.Clients
{
    public class SseClientTest
    {
        [Fact(DisplayName = "Deve criar objeto SseClient")]
        public void ShouldCreateObjectSseClient()
        {
            // arrange - act 
            var result = new SseClient("1", default!);

            // assert
            result.Id.Should().BeEquivalentTo("1");
            result.IsConnected.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve validar propriedades do objeto SseClient")]
        public void ShouldValidateObjectSseClientProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "Id", Type = typeof(string)},
                new() {Name = "IsConnected", Type = typeof(bool)}
            };

            // act - assert
            typeof(SseClient).ValidateProperties(expectedProperties);
        }

        [Fact(DisplayName = "Deve desconectar cliente")]
        public void ShouldDisconnectSseClient()
        {
            // arrange
            var httpContext = new HttpContextTest();
            var client = new SseClient("1", httpContext.Response);

            // act
            var result = client.Disconnect();

            // assert
            httpContext.CountAbortRequest.Should().Be(1);
            result.Should().BeTrue();
            client.IsConnected.Should().BeFalse();
        }

        [Fact(DisplayName = "Não deve desconectar cliente quando o mesmo já estiver desconectado")]
        public void ShouldNotDisconnectClientWhenItIsAlreadyDisconnected()
        {
            // arrange
            var httpContext = new DefaultHttpContext();

            var client = new SseClient("1", httpContext.Response);
            client.Disconnect();

            // act
            var result = client.Disconnect();

            // assert
            result.Should().BeFalse();
            client.IsConnected.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve enviar evento")]
        public async Task ShouldSendEvent()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var client = new SseClient("1", httpContext.Response);

            // act - assert
            await client.Invoking(lnq => lnq.SendEventAsync(new SseEventBytes(new byte[] {10, 20}, 2), CancellationToken.None)).Should()
               .NotThrowAsync();
        }

        [Fact(DisplayName = "Deve lançar exceção ao tentar enviar evento e cliente estiver desconectado")]
        public async Task ShouldThrowExceptionWhenTryingToSendEventAndClientIsDisconnected()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            var client = new SseClient("1", httpContext.Response);
            client.Disconnect();

            // act - assert
            var result = await client.Invoking(lnq => lnq.SendEventAsync(new SseEventBytes(new byte[] {10, 20}, 2), CancellationToken.None)).Should()
               .ThrowAsync<InvalidOperationException>();

            result.WithMessage("The client isn't connected.");
        }
    }
}