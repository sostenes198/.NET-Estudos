using System;
using System.Threading.Tasks;
using Estudos.Redis.Domain;
using Estudos.Redis.Domain.Entities;
using Estudos.Redis.Domain.Redis;
using Estudos.Redis.Domain.Redis.Caches;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Estudos.Redis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController
    {
        private readonly ICacheService<TestCache> _cacheService;
        private readonly PubSubRedis _pubSubRedis;

        public TestController(ICacheService<TestCache> _cacheService, PubSubRedis pubSubRedis)
        {
            this._cacheService = _cacheService;
            _pubSubRedis = pubSubRedis;
        }
        
        [HttpGet("pub")]
        public async Task<IActionResult> Pub(int id)
        {
            await _pubSubRedis.SendTest();
            return new OkObjectResult(default);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _cacheService.TryGetAsync(id.ToString());
            return new OkObjectResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Test test)
        {
            await _cacheService.TrySetAsync(test.Id.ToString(), new() {Id = test.Id, Name = test.Name, Birthday = test.Birthday},
                new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)});
            return new OkResult();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _cacheService.TryRemove(id.ToString());
            return new OkResult();
        }
    }
}