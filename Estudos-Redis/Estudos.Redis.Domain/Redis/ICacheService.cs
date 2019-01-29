using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Estudos.Redis.Domain.Redis
{
    public interface ICacheService<T>
        where T: class
    {
        Task<CacheEntry<T>> TryGetAsync(string key, CancellationToken cancellationToken = default);

        Task TrySetAsync(string key, T value, DistributedCacheEntryOptions options = null, CancellationToken cancellationToken = default);

        Task TryRemove(string key, CancellationToken token = default);
    }
}