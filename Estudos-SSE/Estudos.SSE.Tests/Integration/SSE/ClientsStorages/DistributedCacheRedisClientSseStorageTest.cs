using Estudos.SSE.Core.ClientsStorages;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Integration.Collections;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.SSE.Tests.Integration.SSE.ClientsStorages
{
    [Collection(nameof(SseCollectionTest))]
    public class DistributedCacheRedisClientSseStorageTest : BaseTest
    {
        private const string ClientId = "IntegrationTest-ClientId-1";
        private readonly IClientSseStorage _clientSseStorage;

        public DistributedCacheRedisClientSseStorageTest()
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
            };

            _clientSseStorage = ServiceProvider.GetRequiredService<IClientSseStorage>();
        }

        protected override void DisposeBase()
        {
            Task.Run((Func<Task>) (async () => await _clientSseStorage.RemoveAsync(ClientId))).Wait();
        }

        [Fact(DisplayName = "Deve adicionar id do cliente no cache")]
        public async Task ShouldAddClientIdInCache()
        {
            // arrange - act
            await _clientSseStorage.AddAsync(ClientId);

            var result = await _clientSseStorage.ContainsAsync(ClientId);

            // assert
            result.Should().BeTrue();
        }
        
        [Fact(DisplayName = "Deve retornar falso quando id do cliente não existir no cache")]
        public async Task ShouldReturnFalseWhenClientIdNotFoundInCache()
        {
            // arrange  - act
            var result = await _clientSseStorage.ContainsAsync(ClientId);
            
            // assert
            result.Should().BeFalse();
        }
        
        [Fact(DisplayName = "Deve retornar true quando id do cliente encontrado existir no cache")]
        public async Task ShouldReturnTrueWhenClientIdFoundInCache()
        {
            // arrange
            await _clientSseStorage.AddAsync(ClientId);
            
            // act
            var result = await _clientSseStorage.ContainsAsync(ClientId);
            
            // assert
            result.Should().BeTrue();
        }
        
        [Fact(DisplayName = "Deve atualizar id do cliente no cache")]
        public async Task ShouldUpdateClientIdInCache()
        {
            // arrange - act
            await _clientSseStorage.UpdateAsync(ClientId);

            var result = await _clientSseStorage.ContainsAsync(ClientId);

            // assert
            result.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve remover id do cliente do cache")]
        public async Task ShouldRemoveClientIdFromCache()
        {
            // arrange
            await _clientSseStorage.AddAsync(ClientId);
            
            // act
            await _clientSseStorage.RemoveAsync(ClientId);

            var result = await _clientSseStorage.ContainsAsync(ClientId);
            
            // assert
            result.Should().BeFalse();
        }
    }
}