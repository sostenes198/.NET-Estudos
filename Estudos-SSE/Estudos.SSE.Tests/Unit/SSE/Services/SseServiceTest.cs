using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Reflection;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services;
using Estudos.SSE.Core.Services.Contracts;
using Estudos.SSE.Core.Services.Events;
using Estudos.SSE.Tests.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Tests.Unit.SSE.Services
{
    public class SseServiceTest
    {
        private const string ClientId = "UnitTest-ClientId-1";

        private readonly HttpContextTest _httpContext;
        private readonly SseClient _sseClient;

        private readonly SseEvent _sseEvent = new("UnitTesti-Id", "Event", "Unit Test data");

        private readonly Mock<IClientSseStorage> _clientSseStorageMock;
        private readonly Mock<IServiceProvider> _serviceProviderMock;

        private readonly ISseService _sseService;

        private readonly ConcurrentDictionary<string, SseClient> _clients;

        private int _countClientConnected;
        private int _countClientDisconnected;

        public SseServiceTest()
        {
            _clientSseStorageMock = new Mock<IClientSseStorage>();
            _serviceProviderMock = new Mock<IServiceProvider>();

            var optionsWrapper = new OptionsWrapper<SseServiceOptions>(
                new SseServiceOptions
                {
                    OnClientConnected = (_, _) => { _countClientConnected++; },
                    OnClientDisconnected = (_, _) => { _countClientDisconnected++; }
                });

            _sseService = new SseService(_clientSseStorageMock.Object, _serviceProviderMock.Object, optionsWrapper);
            _clients = GetInMemoryClients();

            _httpContext = GetHttpContext();
            _sseClient = new SseClient(ClientId, _httpContext.Response);
        }

        private ConcurrentDictionary<string, SseClient> GetInMemoryClients()
        {
            var fieldInfo = _sseService.GetType().GetField("_clients", BindingFlags.NonPublic | BindingFlags.Instance)!;

            return (ConcurrentDictionary<string, SseClient>) fieldInfo.GetValue(_sseService)!;
        }

        private static HttpContextTest GetHttpContext() => new();

        [Fact(DisplayName = "Deve criar objeto SseService com eventos")]
        public void ShouldCreateObjectSseServiceWithEvents()
        {
            // arrange - act
            var sseService = new SseService(_clientSseStorageMock.Object, _serviceProviderMock.Object, new OptionsWrapper<SseServiceOptions>(new SseServiceOptions()));

            var fieldInfoClientConnected = sseService.GetType().GetField("ClientConnected", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var clientConnected = (EventHandler<SseClientConnectedArgs>) fieldInfoClientConnected.GetValue(sseService)!;

            var fieldInfoClientDisconnected = sseService.GetType().GetField("ClientDisconnected", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var clientClientDisconnected = (EventHandler<SseClientDisconnectedArgs>) fieldInfoClientDisconnected.GetValue(sseService)!;

            // assert
            clientConnected.Should().NotBeNull();
            clientClientDisconnected.Should().NotBeNull();
        }

        [Fact(DisplayName = "Deve retornar falso quando validar se cliente está conectado e não encontrado")]
        public async Task ShouldReturnFalseWhenClientIsConnectedAndNotFoundClient()
        {
            // arrange - act
            var result = await _sseService.IsClientConnectedAsync(ClientId);

            // assert
            result.Should().BeFalse();
            _clientSseStorageMock.Verify(lnq => lnq.ContainsAsync(ClientId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro quando validar se cliente está conectado e cliente encontrado em memória")]
        public async Task ShouldReturnTrueWhenVerifyClientIsConnectedAndFoundClientInMemory()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            var result = await _sseService.IsClientConnectedAsync(ClientId);

            // assert
            result.Should().BeTrue();
            _clientSseStorageMock.Verify(lnq => lnq.ContainsAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Deve retornar verdadeiro quando validar se cliente está conectado e cliente encontrado no storage")]
        public async Task ShouldReturnTrueWhenVerifyClientIsConnectedAndFoundClientInStorage()
        {
            // arrange 
            _clientSseStorageMock.Setup(lnq => lnq.ContainsAsync(It.IsAny<string>())).ReturnsAsync(true);

            // act
            var result = await _sseService.IsClientConnectedAsync(ClientId);

            // assert
            result.Should().BeTrue();
            _clientSseStorageMock.Verify(lnq => lnq.ContainsAsync(ClientId), Times.Once);
        }

        [Fact(DisplayName = "Deve retornar SseClient nullo quando não encontrado")]
        public void ShouldReturnNullSseClientWhenNotFound()
        {
            // arrange - act
            var result = _sseService.GetClient(ClientId);

            // assert
            result.Should().BeNull();
        }

        [Fact(DisplayName = "Deve retornar SseClient quando encontrado")]
        public void ShouldReturnSseClientWhenFount()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            var result = _sseService.GetClient(ClientId);

            // assert
            result.Should().BeEquivalentTo(_sseClient);
        }

        [Fact(DisplayName = "Deve retornar lista de SseClient vazia")]
        public void ShouldReturnSseClientListEmpty()
        {
            // arrange - act
            var result = _sseService.GetClients();

            // assert
            result.Should().BeEmpty();
        }

        [Fact(DisplayName = "Deve retornar lista de SseClient")]
        public void ShouldReturnSseClientList()
        {
            // arrange
            var sseClient2 = new SseClient("UnitTest-ClientId-2", GetHttpContext().Response);

            var expectedResult = new ReadOnlyCollection<SseClient>(
                new List<SseClient>
                {
                    _sseClient,
                    sseClient2
                });

            _clients.TryAdd(ClientId, _sseClient);
            _clients.TryAdd(sseClient2.Id, sseClient2);

            // act
            var result = _sseService.GetClients();

            // assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact(DisplayName = "Deve conectar cliente")]
        public async Task ShouldConnectClient()
        {
            // arrange - act 
            await _sseService.ConnectClientAsync(_sseClient);
            var result = _sseService.GetClient(ClientId);

            // assert
            result.Should().BeEquivalentTo(_sseClient);
            _countClientConnected.Should().Be(1);
            _clientSseStorageMock.Verify(lnq => lnq.AddAsync(ClientId), Times.Once);
        }

        [Fact(DisplayName = "Deve desconectar cliente")]
        public async Task ShouldDisconnectClient()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            await _sseService.DisconnectClientAsync(_sseClient);

            // assert
            _countClientDisconnected.Should().Be(1);
            _clientSseStorageMock.Verify(lnq => lnq.RemoveAsync(ClientId), Times.Once);
            _httpContext.CountAbortRequest.Should().Be(1);
        }

        [Fact(DisplayName = "Deve enviar evento em string sem cancellationToken")]
        public async Task ShouldSendEventInStringWithoutCancellationToken()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            await _sseService.SendEventAsync(ClientId, "Unit Text");

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Once);
            ((StreamTest) _httpContext.Response.Body).CountWriteAsync.Should().Be(1);
        }

        [Fact(DisplayName = "Não deve enviar evento em string sem cancellationToken quando cliente desconectado")]
        public async Task ShouldNotSendEventInStringWithoutCancellationTokenWhenClientDisconnected()
        {
            // arrange
            var clientId = "UnitTest-SendEvent-1";
            var sseClient = new SseClient(clientId, GetHttpContext().Response);
            sseClient.Disconnect();
            _clients.TryAdd(clientId, sseClient);

            // act
            await _sseService.SendEventAsync(clientId, "Unit Text");

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Não deve enviar evento com sucesso em string sem cancellationToken")]
        public async Task ShouldNotSendEventInStringWithoutCancellationToken()
        {
            // arrange - act
            await _sseService.SendEventAsync(ClientId, "Unit Text");

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Deve enviar evento em string com cancellationToken")]
        public async Task ShouldSendEventInStringWithCancellationToken()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            await _sseService.SendEventAsync(ClientId, "Unit Text", CancellationToken.None);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Once);
            ((StreamTest) _httpContext.Response.Body).CountWriteAsync.Should().Be(1);
        }

        [Fact(DisplayName = "Não deve enviar evento em string com cancellationToken quando cliente desconectado")]
        public async Task ShouldNotSendEventInStringWithCancellationTokenWhenClientDisconnected()
        {
            // arrange
            var clientId = "UnitTest-SendEvent-1";
            var sseClient = new SseClient(clientId, GetHttpContext().Response);
            sseClient.Disconnect();
            _clients.TryAdd(clientId, sseClient);

            // act
            await _sseService.SendEventAsync(clientId, "Unit Text", CancellationToken.None);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Não deve enviar evento com sucesso em string com cancellationToken")]
        public async Task ShouldNotSendEventInStringWithCancellationToken()
        {
            // arrange - act
            await _sseService.SendEventAsync(ClientId, "Unit Text", CancellationToken.None);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Deve lançar exceção ao enviar tentar enviar evento em string com token cancelado")]
        public async Task ShouldThrowExceptionWhenSendingTryToSendEventInStringWithTokenCanceled()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // act - assert
            await _sseService.Invoking(lnq => lnq.SendEventAsync(ClientId, "Unit Text", cancellationTokenSource.Token))
               .Should()
               .ThrowAsync<Exception>();

            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Deve enviar evento em SseEvent sem cancellationToken")]
        public async Task ShouldSendEventInSseEventWithoutCancellationToken()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            await _sseService.SendEventAsync(ClientId, _sseEvent);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Once);
            ((StreamTest) _httpContext.Response.Body).CountWriteAsync.Should().Be(1);
        }

        [Fact(DisplayName = "Não deve enviar evento em SseEvent sem cancellationToken quando cliente desconectado")]
        public async Task ShouldNotSendEventInSseEventWithoutCancellationTokenWhenClientDisconnected()
        {
            // arrange
            var clientId = "UnitTest-SendEvent-1";
            var sseClient = new SseClient(clientId, GetHttpContext().Response);
            sseClient.Disconnect();
            _clients.TryAdd(clientId, sseClient);

            // act
            await _sseService.SendEventAsync(clientId, _sseEvent);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Não deve enviar evento em SseEvent sem cancellationToken")]
        public async Task ShouldNotSendEventInSseEventWithoutCancellationToken()
        {
            // arrange - act
            await _sseService.SendEventAsync(ClientId, _sseEvent);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Deve enviar evento em SseEvent com cancellationToken")]
        public async Task ShouldSendEventInSseEventWithCancellationToken()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            // act
            await _sseService.SendEventAsync(ClientId, _sseEvent, CancellationToken.None);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Once);
            ((StreamTest) _httpContext.Response.Body).CountWriteAsync.Should().Be(1);
        }

        [Fact(DisplayName = "Não deve enviar evento em SseEvent com cancellationToken quando cliente desconectado")]
        public async Task ShouldNotSendEventInSseEventWithCancellationTokenWhenClientDisconnected()
        {
            // arrange
            var clientId = "UnitTest-SendEvent-1";
            var sseClient = new SseClient(clientId, GetHttpContext().Response);
            sseClient.Disconnect();
            _clients.TryAdd(clientId, sseClient);

            // act
            await _sseService.SendEventAsync(clientId, _sseEvent, CancellationToken.None);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Não deve enviar evento com sucesso em SseEvent com cancellationToken")]
        public async Task ShouldNotSendEventInSseEventWithCancellationToken()
        {
            // arrange - act
            await _sseService.SendEventAsync(ClientId, _sseEvent, CancellationToken.None);

            // assert
            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }

        [Fact(DisplayName = "Deve lançar exceção ao enviar tentar enviar evento em SseEvent com token cancelado")]
        public async Task ShouldThrowExceptionWhenSendingTryToSendEventInSseEventWithTokenCanceled()
        {
            // arrange
            _clients.TryAdd(ClientId, _sseClient);

            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();

            // act - assert
            await _sseService.Invoking(lnq => lnq.SendEventAsync(ClientId, _sseEvent, cancellationTokenSource.Token))
               .Should()
               .ThrowAsync<Exception>();

            _clientSseStorageMock.Verify(lnq => lnq.UpdateAsync(ClientId), Times.Never);
        }
    }
}