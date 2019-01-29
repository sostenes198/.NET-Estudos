using System.Net;
using System.Text.Json;
using Estudos.SSE.Core.BackgroundServices;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Services.Contracts;
using Estudos.SSE.Extensions;
using Estudos.SSE.Tests.Integration.Collections;
using Estudos.SSE.Tests.Integration.SSE.SSE.IntegrationTests.Fixture;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Tests.Integration.SSE.SSE.IntegrationTests
{
    [Collection(nameof(SseCollectionTest))]
    public class SseTest : BaseTest
    {
        private const string SseEndpoint = "sse-endpoint";

        private const string ClientId1 = "Sse-IntegrationTest-1";
        private const string ClientId2 = "Sse-IntegrationTest-2";

        private int _countOnClientConnected;
        private int _countOnClientDisconnected;
        private int _countOnPrepareAccept;

        private readonly IClientSseStorage _clientSseStorage;
        private readonly ISseService _sseService;

        public SseTest()
        {
            ConfigureServices += (context, collection) =>
            {
                collection.TryAddSingleton<IDistributedCache>(
                    _ => new RedisCache(
                        new RedisCacheOptions
                        {
                            Configuration = context.Configuration["Redis"]
                        }));

                collection.AddSse(
                        opt =>
                        {
                            opt.WithAuthorization<IntegrationTestAuthorizationSse>()
                               .WithClientIdProvider<IntegrationTestClientIdProvider>();
                        },
                        opt =>
                        {
                            opt.OnClientConnected += (_, _) => _countOnClientConnected++;
                            opt.OnClientDisconnected += (_, _) => _countOnClientDisconnected++;
                            opt.OnPrepareAccept += _ => _countOnPrepareAccept++;
                            opt.CloseConnectionsInSecondsInterval = 60;
                            opt.MaxTimeCacheInMinutes = 2;
                        })
                   .AddKeepAliveSseConnection();
            };

            Configure += builder => { builder.UseSse($"/{SseEndpoint}"); };

            _clientSseStorage = ServiceProvider.GetRequiredService<IClientSseStorage>();
            _sseService = ServiceProvider.GetRequiredService<ISseService>();

            ClearSseStorage();
        }

        protected override void DisposeBase()
        {
            ClearSseStorage();
        }

        private void ClearSseStorage()
        {
            Task.Run(
                    async () =>
                    {
                        await _clientSseStorage.RemoveAsync(ClientId1);
                        await _clientSseStorage.RemoveAsync(ClientId2);
                    })
               .Wait();
        }

        private void DisableKeepAliveSseConnection()
        {
            Task.Run(async () => await ServiceProvider.GetServices<IHostedService>().First(lnq => lnq is KeepAliveSseConnectionBackgroundService).StopAsync(CancellationToken.None)).Wait();
        }

        private HttpClient GetHttpClient(string clientId)
        {
            var client = Client;
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Login", "IntegrationTest");
            client.DefaultRequestHeaders.Add("Password", "123456789");
            client.DefaultRequestHeaders.Add("ClientId", clientId);

            return client;
        }

        [Fact(DisplayName = "Deve retornar Unathorized quando client não autorizado para abrir conexão SSE")]
        public async Task ShouldReturnUnauthorizedWhenClientNotAuthorizedToOpenSseConnection()
        {
            // arrange
            DisableKeepAliveSseConnection();
            var client = GetHttpClient(string.Empty);
            client.DefaultRequestHeaders.Remove("Login");
            client.DefaultRequestHeaders.Remove("Password");

            // act
            var result = await client.GetAsync(SseEndpoint);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact(DisplayName = "Deve retornar Conflict quando já existir um cliente conectado com o mesmo id informado")]
        public async Task ShouldReturnConflictWhenThereIsAlreadyAClientConnectedWithTheSameIdInformed()
        {
            // arrange
            DisableKeepAliveSseConnection();
            var client = GetHttpClient(ClientId1);
            await _clientSseStorage.AddAsync(ClientId1);

            // act
            var result = await client.GetAsync(SseEndpoint);

            // assert
            result.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact(DisplayName = "Deve abrir conexão SSE com sucesso")]
        public async Task ShouldOpenSuccessfullySseConnection()
        {
            // arrange
            var httpClient = GetHttpClient(ClientId1);

            // act
            await using (await httpClient.GetStreamAsync(SseEndpoint))
            {
            }

            await Task.Delay(TimeSpan.FromSeconds(1));

            // assert
            (await _clientSseStorage.ContainsAsync(ClientId1)).Should().BeFalse();
            _sseService.GetClients().Should().BeEmpty();
            _countOnClientConnected.Should().Be(1);
            _countOnClientDisconnected.Should().Be(1);
            _countOnPrepareAccept.Should().Be(1);
        }

        [Fact(DisplayName = "Deve validar envio de eventos pelo serviço de sse")]
        public async Task ShouldValidateSendEventsWithSseService()
        {
            // arrange
            DisableKeepAliveSseConnection();
            var httpClient = GetHttpClient(ClientId1);

            var result = new List<string>();

            var expectedResult = new List<string>
            {
                "data: IntegrationTest-Texto-1",
                "data: IntegrationTest-Texto-2",
                "event: Event-ClientId-1-Event1 data: {\"TestPropery\":1}",
                "id: 1 event: Event-ClientId-1-Event2 data: IntegrationTest-SseEvent-Text"
            };

            // act
            var timer = new System.Timers.Timer(2000);

            timer.Elapsed += (_, _) =>
            {
                _sseService.SendEventAsync(ClientId1, "IntegrationTest-Texto-1");
                _sseService.SendEventAsync(ClientId1, "IntegrationTest-Texto-2", CancellationToken.None);
                _sseService.SendEventAsync(ClientId1, new SseEvent("Event-ClientId-1-Event1", JsonSerializer.Serialize(new {TestPropery = 1})));
                _sseService.SendEventAsync(ClientId1, new SseEvent("1", "Event-ClientId-1-Event2", "IntegrationTest-SseEvent-Text"), CancellationToken.None);
                timer.Stop();
            };

            timer.Start();

            await using var stream = await httpClient.GetStreamAsync(SseEndpoint);

            using var reader = new StreamReader(stream);

            while (result.Count < 4)
                result.Add(await GetLineAsync(reader));

            // assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Fact(DisplayName = "Deve testar envio para mais de uma canal SSE em aberto")]
        public async Task ShouldTestSendingToMoreThanOneOpenSseChannel()
        {
            // arrange
            DisableKeepAliveSseConnection();
            var httpClient1 = GetHttpClient(ClientId1);
            var httpClient2 = GetHttpClient(ClientId2);

            var expectedResultClientId1 = "IntegrationTest-Texto-Client1";
            var expectedResultClientId2 = "IntegrationTest-Texto-Client2";

            string resultClientId1;
            string resultClientId2;

            // act
            var timer = new System.Timers.Timer(2000);

            timer.Elapsed += (_, _) =>
            {
                _sseService.SendEventAsync(ClientId1, expectedResultClientId1);
                _sseService.SendEventAsync(ClientId2, expectedResultClientId2);
                timer.Stop();
            };

            timer.Start();

            var streamClient1 = await httpClient1.GetStreamAsync(SseEndpoint);
            var streamClient2 = await httpClient2.GetStreamAsync(SseEndpoint);

            using (var reader = new StreamReader(streamClient1))
            {
                resultClientId1 = await GetLineAsync(reader);
            }

            using (var reader = new StreamReader(streamClient2))
            {
                resultClientId2 = await GetLineAsync(reader);
            }

            // assert
            resultClientId1.Should().BeEquivalentTo($"data: {expectedResultClientId1}");
            resultClientId2.Should().BeEquivalentTo($"data: {expectedResultClientId2}");
        }

        private static async Task<string> GetLineAsync(StreamReader reader)
        {
            string line;
            string completedEvent = string.Empty;

            do
            {
                line = await reader.ReadLineAsync() ?? string.Empty;

                if (line != string.Empty)
                    completedEvent += line + " ";
            } while (line != string.Empty);

            return completedEvent.Trim();
        }
    }
}