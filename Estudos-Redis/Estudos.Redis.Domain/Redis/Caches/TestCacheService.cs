using Estudos.Redis.Domain.Serializer;
using Microsoft.Extensions.Caching.Distributed;

namespace Estudos.Redis.Domain.Redis.Caches
{
    public class TestCacheService : CacheService<TestCache>
    {
        public TestCacheService(ICacheSerializer cacheSerializer, IDistributedCache distributedCache) : base(cacheSerializer, distributedCache)
        {
        }

        protected override string Namespace => "test";
    }
}