using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Integration.Collections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Tests.Integration.SSE.Options
{
    [Collection(nameof(SseCollectionTest))]
    public class DistributedCacheClientSseStorageOptionsTest : BaseTest
    {
        public DistributedCacheClientSseStorageOptionsTest()
        {
            ConfigureServices += (_, collection) =>
            {
                collection.Configure<DistributedCacheClientSseStorageOptions>(opt => { opt.MaxTimeCacheInMinutes = 10; });
            };
        }
        [Fact(DisplayName = "Deve validar configurações objeto DistributedCacheClientSseStorageOptions")]
        public void ShouldValidateConfigurationsObjectDistributedCacheClientSseStorageOptions()
        {
            // arrange - act
            var result = ServiceProvider.GetRequiredService<IOptions<DistributedCacheClientSseStorageOptions>>().Value;
            
            // assert
            result.MaxTimeCacheInMinutes.Should().Be(10);
        }
    }
}