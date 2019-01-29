using Estudos.Redis.Domain.Redis;
using Estudos.Redis.Domain.Redis.Caches;
using Estudos.Redis.Domain.Serializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using StackExchange.Redis;

namespace Estudos.Redis.Domain
{
    public static class Bootstrap
    {
        public static void InitializeDomain(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("ConexaoRedis");
                options.InstanceName = "Estudos-Redis";
            });
            services.TryAddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(configuration["ConnectionStrings:ConexaoRedis"]));
            
            services.TryAddSingleton<ICacheSerializer, CacheSerializer>();
            services.TryAddScoped<ICacheService<TestCache>, TestCacheService>();
            services.TryAddSingleton<PubSubRedis>();
        }
    }
}