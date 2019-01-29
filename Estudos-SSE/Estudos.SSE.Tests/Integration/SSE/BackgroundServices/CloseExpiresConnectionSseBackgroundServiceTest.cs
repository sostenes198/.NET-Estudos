using Estudos.SSE.Core.BackgroundServices;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.ClientsStorages;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services;
using Estudos.SSE.Core.Services.Contracts;
using Estudos.SSE.Tests.Integration.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Estudos.SSE.Tests.Integration.SSE.BackgroundServices
{
    [Collection(nameof(SseCollectionTest))]
    public class CloseExpiresConnectionSseBackgroundServiceTest : BaseTest
    {
        private readonly IHostedService _hostedService;
        private readonly ISseService _sseService;
        private readonly IClientSseStorage _clientSseStorage;

        public CloseExpiresConnectionSseBackgroundServiceTest()
        {
            ConfigureServices += (context, collection) =>
            {
                collection.Configure<DistributedCacheClientSseStorageOptions>(opt => { opt.MaxTimeCacheInMinutes = 10; });

                collection.TryAddSingleton<IDistributedCache>(
                    _ => new RedisCache(
                        new RedisCacheOptions
                        {
                            Configuration = context.Configuration["Redis"]
                        }));

                collection.TryAddSingleton<IClientSseStorage, DistributedCacheClientSseStorage>();
                collection.TryAddSingleton<SseService>();
                collection.TryAddSingleton<ISseService>(provider => provider.GetRequiredService<SseService>());
                collection.TryAddSingleton<ISseServiceClientManager>(provider => provider.GetRequiredService<SseService>());
                collection.TryAddSingleton<ISseServiceSendEvent>(provider => provider.GetRequiredService<SseService>());

                collection.Configure<CloseExpiresConnectionOptions>(opt => opt.CloseConnectionsInSecondsInterval = 5);
                collection.TryAddSingleton<CloseExpiresConnectionSseBackgroundService>();
                collection.AddSingleton<IHostedService>(provider => provider.GetRequiredService<CloseExpiresConnectionSseBackgroundService>());
            };

            _hostedService = ServiceProvider.GetRequiredService<CloseExpiresConnectionSseBackgroundService>();
            _sseService = ServiceProvider.GetRequiredService<ISseService>();
            _clientSseStorage = ServiceProvider.GetRequiredService<IClientSseStorage>();
        }

        protected override void DisposeBase()
        {
            Task.Run(async () => await _hostedService.StopAsync(CancellationToken.None));
        }

        [Fact(DisplayName = "Deve valiar CloseExpiresConnectionBackgroundService")]
        public async Task ShouldValidateCloseExpiresConnectionBackgroundService()
        {
            // arrange
            var sseClient1 = new SseClient("CloseExpiresConnectionBackgroundService-IntegrationTest-1", new DefaultHttpContext().Response);
            var sseClient2 = new SseClient("CloseExpiresConnectionBackgroundService-IntegrationTest-2", new DefaultHttpContext().Response);
            await _sseService.ConnectClientAsync(sseClient1);
            await _sseService.ConnectClientAsync(sseClient2);

            // act
            await _hostedService.StartAsync(CancellationToken.None);

            await _clientSseStorage.RemoveAsync(sseClient1.Id);
            await _clientSseStorage.RemoveAsync(sseClient2.Id);

            // assert
            while (await _sseService.IsClientConnectedAsync(sseClient1.Id) && await _sseService.IsClientConnectedAsync(sseClient2.Id))
                await Task.Delay(100);

            await _hostedService.StopAsync(CancellationToken.None);
        }
    }
}