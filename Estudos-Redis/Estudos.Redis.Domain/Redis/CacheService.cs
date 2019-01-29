using System.Threading;
using System.Threading.Tasks;
using Estudos.Redis.Domain.Serializer;
using Microsoft.Extensions.Caching.Distributed;

namespace Estudos.Redis.Domain.Redis
{
    public abstract class CacheService<T> : ICacheService<T>
        where T : class
    {
        private readonly ICacheSerializer _cacheSerializer;
        private readonly IDistributedCache _distributedCache;

        protected abstract string Namespace { get; }

        public CacheService(ICacheSerializer cacheSerializer, IDistributedCache distributedCache)
        {
            _cacheSerializer = cacheSerializer;
            _distributedCache = distributedCache;
        }

        public async Task<CacheEntry<T>> TryGetAsync(string key, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildKey(key);
            var distributedCacheValue = await TryGetCacheAsync(cacheKey, cancellationToken);
            return TryGetCacheResult(distributedCacheValue);
        }
        
        private async Task<byte[]> TryGetCacheAsync(string cacheKey, CancellationToken token)
        {
            try
            {
                return await _distributedCache.GetAsync(cacheKey, token);
            }
            catch
            {
                return new byte[0];
            }
        }
        
        private CacheEntry<T> TryGetCacheResult(byte[] cacheResult)
        {
            if(cacheResult == default || cacheResult.Length <= 0)
                return CacheEntry<T>.NotFound;
            try
            {
                var entityCache  = _cacheSerializer.Deserialize<T>(cacheResult);
                return CacheEntry<T>.Create(true, entityCache);
            }
            catch
            {
                return CacheEntry<T>.NotFound;
            }
        }

        public async Task TrySetAsync(string key, T value, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default)
        {
            var cacheKey = BuildKey(key);
            var distributedCacheValue = _cacheSerializer.Serialize(value);
            try
            {
                await _distributedCache.SetAsync(cacheKey, distributedCacheValue, options, cancellationToken);
            }
            catch
            {
                // ignore
            }
        }

        public async Task TryRemove(string key, CancellationToken token)
        {
            var cacheKey = BuildKey(key);
            try
            {
                await _distributedCache.RemoveAsync(cacheKey, token);
            }
            catch
            {
                //
            }
        }

        protected string BuildKey(string key) => $"estudos.redis.{Namespace.ToLower().Trim()}.{key.ToLower().Trim()}";
    }
}