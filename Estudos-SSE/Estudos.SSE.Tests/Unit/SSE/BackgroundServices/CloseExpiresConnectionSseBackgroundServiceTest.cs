using System.Reflection;
using Estudos.SSE.Core.BackgroundServices;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Extensions;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace Estudos.SSE.Tests.Unit.SSE.BackgroundServices
{
    public class CloseExpiresConnectionSseBackgroundServiceTest : IDisposable
    {
        private readonly Mock<ISseServiceClientManager> _sseServiceClientManagerMock;
        private readonly Mock<IClientSseStorage> _clientSseStorageMock;
        private readonly IHostedService _hostedService;
        private readonly CancellationTokenSource _stoppingCts;

        private bool _timeoutTest;

        private readonly SseClient _sseClient1;
        private readonly SseClient _sseClient2;

        public CloseExpiresConnectionSseBackgroundServiceTest()
        {
            _sseServiceClientManagerMock = new Mock<ISseServiceClientManager>();
            _clientSseStorageMock = new Mock<IClientSseStorage>();

            var options = new OptionsWrapper<CloseExpiresConnectionOptions>(
                new CloseExpiresConnectionOptions
                {
                    CloseConnectionsInSecondsInterval = 1
                });

            _hostedService = new CloseExpiresConnectionSseBackgroundService(_sseServiceClientManagerMock.Object, _clientSseStorageMock.Object, options);
            _stoppingCts = GetCancellationTokenSource();

            _sseClient1 = new SseClient("CloseExpiresConnectionBackgroundService-UnitTest-ClientId-1", new DefaultHttpContext().Response);
            _sseClient2 = new SseClient("CloseExpiresConnectionBackgroundService-UnitTest-ClientId-2", new DefaultHttpContext().Response);
            
            InitializeTimerToCloseTimeoutTests();
        }
        
        public void Dispose()
        {
            _timeoutTest.Should().BeFalse();
        }

        private void InitializeTimerToCloseTimeoutTests()
        {
            var timer = new System.Timers.Timer(5000);

            timer.Elapsed += (_, _) =>
            {
                _timeoutTest = true;
                ((IDisposable) _hostedService).Dispose();
                timer.Stop();
            };

            timer.Start();
        }

        private CancellationTokenSource GetCancellationTokenSource()
        {
            var property = _hostedService.GetType().GetField("_stoppingCts", BindingFlags.Instance | BindingFlags.NonPublic)!;

            return (CancellationTokenSource) property.GetValue(_hostedService)!;
        }

        [Fact(DisplayName = "Deve remover conexões expiradas")]
        public async Task ShouldRemoveExpiredConnections()
        {
            // arrange
            _sseServiceClientManagerMock.SetupSequence(lnq => lnq.GetClients())
               .Returns(
                    new List<SseClient>
                    {
                        _sseClient1,
                        _sseClient2
                    })
               .Returns(Array.Empty<SseClient>())
               .Returns(
                    () =>
                    {
                        ((IDisposable) _hostedService).Dispose();

                        return new List<SseClient>();
                    });

            _clientSseStorageMock.Setup(lnq => lnq.ContainsAsync(_sseClient1.Id)).Returns(Task.FromResult(false));
            _clientSseStorageMock.Setup(lnq => lnq.ContainsAsync(_sseClient2.Id)).Returns(Task.FromResult(true));

            // act
            await _hostedService.StartAsync(CancellationToken.None);
            await _stoppingCts.Token.WaitAsync();

            // assert
            ValidateAsserts(3, 1, 1, 1, 0);
        }

        [Fact(DisplayName = "Não deve iniciar background service quando token já estiver cancelado")]
        public async Task ShouldNotInitializeBackgroundServiceWhenTokenCanceled()
        {
            // arrange
            ((IDisposable) _hostedService).Dispose();

            // act
            await _hostedService.StartAsync(CancellationToken.None);

            // assert
            ValidateAsserts(0, 0, 0, 0, 0);
        }

        [Fact(DisplayName = "Não deve pausar background service quando não existir tarefa em execução")]
        public async Task ShouldNotUseBackgroundServiceWhenThereIsNoTaskRunning()
        {
            // arrange - act
            await _hostedService.StopAsync(CancellationToken.None);

            // assert
            _stoppingCts.IsCancellationRequested.Should().BeFalse();
        }

        [Fact(DisplayName = "Deve pausar background service quando existir tarefa em execução")]
        public async Task ShouldUseBackgroundServiceWhenThereIsTaskRunning()
        {
            // arrange
            _sseServiceClientManagerMock.Setup(lnq => lnq.GetClients()).Returns(Array.Empty<SseClient>());

            // act
            await _hostedService.StartAsync(CancellationToken.None);
            await _hostedService.StopAsync(CancellationToken.None);

            // assert
            _stoppingCts.IsCancellationRequested.Should().BeTrue();
        }

        private void ValidateAsserts(
            int callCountGetClients,
            int callCountContainsAsyncClient1,
            int callCountContainsAsyncClient2,
            int callCountDisconnectClientAsync1,
            int callCountDisconnectClientAsync2)
        {
            _sseServiceClientManagerMock.Verify(lnq => lnq.GetClients(), Times.Exactly(callCountGetClients));
            _clientSseStorageMock.Verify(lnq => lnq.ContainsAsync(_sseClient1.Id), Times.Exactly(callCountContainsAsyncClient1));
            _clientSseStorageMock.Verify(lnq => lnq.ContainsAsync(_sseClient2.Id), Times.Exactly(callCountContainsAsyncClient2));
            _sseServiceClientManagerMock.Verify(lnq => lnq.DisconnectClientAsync(_sseClient1), Times.Exactly(callCountDisconnectClientAsync1));
            _sseServiceClientManagerMock.Verify(lnq => lnq.DisconnectClientAsync(_sseClient2), Times.Exactly(callCountDisconnectClientAsync2));
        }
    }
}