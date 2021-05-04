using System;
using System.Threading.Tasks;
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

        public TestController(ICacheService<TestCache> _cacheService)
        {
            this._cacheService = _cacheService;
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
                new DistributedCacheEntryOptions {AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)});
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