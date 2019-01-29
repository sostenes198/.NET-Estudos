using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Options
{
    public class DistributedCacheClientSseStorageOptionsTest
    {
        [Theory(DisplayName = "Deve criar objeto DistributedCacheClientSseStorageOptions")]
        [InlineData(0, 5)]
        [InlineData(50, 50)]
        public void ShouldCreateObjectDistributedCacheClientSseStorageOptions(int maxTimeCacheInMinutes, int expectedMaxTimeCacheInMinutes)
        {
            // arrange - act
            var result = new DistributedCacheClientSseStorageOptions
            {
                MaxTimeCacheInMinutes = maxTimeCacheInMinutes
            };

            // assert
            result.MaxTimeCacheInMinutes.Should().Be(expectedMaxTimeCacheInMinutes);
        }

        [Fact(DisplayName = "Deve validar propriedades do objeto DistributedCacheClientSseStorageOptions")]
        public void ShouldValidateObjectDistributedCacheClientSseStorageOptionsProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "MaxTimeCacheInMinutes", Type = typeof(int)}
            };

            // act - assert
            typeof(DistributedCacheClientSseStorageOptions).ValidateProperties(expectedProperties);
        }
    }
}