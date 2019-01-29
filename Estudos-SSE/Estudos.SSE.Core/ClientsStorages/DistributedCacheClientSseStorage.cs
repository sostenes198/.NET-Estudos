using System;
using System.Threading.Tasks;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Core.ClientsStorages
{
    internal sealed class DistributedCacheClientSseStorage : IClientSseStorage
    {
        private readonly int _maxTimeCacheInMinutes;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly IDistributedCache _cache;
        
        private readonly byte[] _dummyItem = new byte[1];

        public DistributedCacheClientSseStorage(
            IOptions<DistributedCacheClientSseStorageOptions> options,
            IHostEnvironment hostEnvironment,
            IDistributedCache cache)
        {
            _hostEnvironment = hostEnvironment;
            _cache = cache;
            _maxTimeCacheInMinutes = options.Value.MaxTimeCacheInMinutes;
        }

        public Task AddAsync(string clientId) => _cache.SetAsync(GetKey(clientId), _dummyItem, GetDistributedCacheEntryOptions());

        public async Task<bool> ContainsAsync(string clientId) => await _cache.GetAsync(GetKey(clientId)).ConfigureAwait(false) is not null;

        public Task UpdateAsync(string clientId) =>_cache.SetAsync(GetKey(clientId), _dummyItem, GetDistributedCacheEntryOptions());

        public async Task RemoveAsync(string clientId) => await _cache.RemoveAsync(GetKey(clientId));
        
        private string GetKey(in string clientId) => $"{_hostEnvironment.ApplicationName}.Sse.{clientId}";

        private DistributedCacheEntryOptions GetDistributedCacheEntryOptions() => new() {AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(_maxTimeCacheInMinutes)};
    }
}