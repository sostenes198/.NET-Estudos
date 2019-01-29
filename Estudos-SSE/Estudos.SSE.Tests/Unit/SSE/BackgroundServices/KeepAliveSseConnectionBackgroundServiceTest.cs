using System.Reflection;
using Estudos.SSE.Core.BackgroundServices;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Extensions;
using Estudos.SSE.Core.Services.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Moq;

namespace Estudos.SSE.Tests.Unit.SSE.BackgroundServices
{
    public class KeepAliveSseConnectionBackgroundServiceTest : IDisposable
    {
        private readonly Mock<ISseServiceClientManager> _sseServiceClientManagerMock;
        private readonly Mock<ISseServiceSendEvent> _sseServiceSendEventMock;
        private readonly IHostedService _hostedService;
        private readonly CancellationTokenSource _stoppingCts;
        private readonly SseEvent _keepAliveSseEvent;

        private bool _timeoutTest;

        private readonly SseClient _sseClient1;
        private readonly SseClient _sseClient2;

        public KeepAliveSseConnectionBackgroundServiceTest()
        {
            _sseServiceClientManagerMock = new Mock<ISseServiceClientManager>();
            _sseServiceSendEventMock = new Mock<ISseServiceSendEvent>();

            _hostedService = new KeepAliveSseConnectionBackgroundService(_sseServiceClientManagerMock.Object, _sseServiceSendEventMock.Object);
            _stoppingCts = GetCancellationTokenSource();
            _keepAliveSseEvent = GetKeepAliveSseEvent();

            _sseClient1 = new SseClient("KeepAliveSseConnectionTest-UnitTest-ClientId-1", new DefaultHttpContext().Response);
            _sseClient2 = new SseClient("KeepAliveSseConnectionTest-UnitTest-ClientId-2", new DefaultHttpContext().Response);

            SetKeepAliveConnectionInSecondsInterval();
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

        private void SetKeepAliveConnectionInSecondsInterval()
        {
            var property = _hostedService.GetType().GetField("_keepAliveConnectionInSecondsInterval", BindingFlags.Instance | BindingFlags.NonPublic)!;
            property.SetValue(_hostedService, 1);
        }

        private CancellationTokenSource GetCancellationTokenSource()
        {
            var property = _hostedService.GetType().GetField("_stoppingCts", BindingFlags.Instance | BindingFlags.NonPublic)!;

            return (CancellationTokenSource) property.GetValue(_hostedService)!;
        }

        private SseEvent GetKeepAliveSseEvent()
        {
            var property = _hostedService.GetType().GetField("KeepAliveSseEvent", BindingFlags.Static | BindingFlags.NonPublic)!;

            return (SseEvent) property.GetValue(_hostedService)!;
        }

        [Fact(DisplayName = "Deve validar evento KeepAlive")]
        public void ShouldValidateKeepAliveEvent()
        {
            // arrange
            var expectedResult = new SseEvent("KEEPALIVE", "PING");

            // act - assert
            _keepAliveSseEvent.Should().BeEquivalentTo(expectedResult);
        }

        [Fact(DisplayName = "Deve manter conexões em aberto")]
        public async Task ShouldKeepAliveOpenConnections()
        {
            // arrange
            _sseServiceSendEventMock.Setup(
                    lnq => lnq.SendEventAsync(
                        It.IsAny<string>(),
                        It.IsAny<SseEvent>(),
                        It.IsAny<CancellationToken>()))
               .Returns(() => Task.Delay(1000));

            _sseServiceClientManagerMock.SetupSequence(lnq => lnq.GetClients())
               .Returns(
                    () =>
                    {
                        ((IDisposable) _hostedService).Dispose();

                        return new List<SseClient>
                        {
                            _sseClient1,
                            _sseClient2
                        };
                    });

            // act
            await _hostedService.StartAsync(CancellationToken.None);
            await _stoppingCts.Token.WaitAsync();

            // assert
            ValidateAsserts(1, 1);
        }

        [Fact(DisplayName = "Não deve iniciar background service quando token já estiver cancelado")]
        public async Task ShouldNotInitializeBackgroundServiceWhenTokenCanceled()
        {
            // arrange
            ((IDisposable) _hostedService).Dispose();

            // act
            await _hostedService.StartAsync(CancellationToken.None);

            // assert
            ValidateAsserts(0, 0);
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

        private void ValidateAsserts(int countCallSendEventAsyncClientId1, int countCallSendEventAsyncClientId2)
        {
            _sseServiceSendEventMock.Verify(lnq => lnq.SendEventAsync(_sseClient1.Id, _keepAliveSseEvent, It.IsAny<CancellationToken>()), Times.Exactly(countCallSendEventAsyncClientId1));
            _sseServiceSendEventMock.Verify(lnq => lnq.SendEventAsync(_sseClient2.Id, _keepAliveSseEvent, It.IsAny<CancellationToken>()), Times.Exactly(countCallSendEventAsyncClientId2));
        }
    }
}