using Estudos.SSE.Core.ClientsStorages;
using Estudos.SSE.Core.ClientsStorages.Contracts;
using Estudos.SSE.Core.Options;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Moq;

namespace Estudos.SSE.Tests.Unit.SSE.ClientsStorages
{
    public class DistributedCacheClientSseStorageTest
    {
        private const int MaxTimeCacheInMinutes = 10;
        private const string ApplicationName = "UnitTests";
        private const string ClientId = "ClientId-10";

        private readonly Mock<IDistributedCache> _cacheMock;

        private readonly IClientSseStorage _clientSseStorage;

        public DistributedCacheClientSseStorageTest()
        {
            var optionsWrapper = new OptionsWrapper<DistributedCacheClientSseStorageOptions>(
                new DistributedCacheClientSseStorageOptions
                {
                    MaxTimeCacheInMinutes = MaxTimeCacheInMinutes
                });

            var hostEnvironment = new Mock<IHostEnvironment>();
            hostEnvironment.Setup(lnq => lnq.ApplicationName).Returns(ApplicationName);
            
            _cacheMock = new Mock<IDistributedCache>();

            _clientSseStorage = new DistributedCacheClientSseStorage(optionsWrapper, hostEnvironment.Object, _cacheMock.Object);
        }

        private static string GetCacheKey() => $"{ApplicationName}.Sse.{ClientId}";

        [Fact(DisplayName = "Deve adicionar cliente ao cache")]
        public async Task ShouldSaveClientInCache()
        {
            // arrange - act
            await _clientSseStorage.AddAsync(ClientId);

            // assert
           ValidateCacheSetAsync();
        }

        [Theory(DisplayName = "Deve validar que contem client no cache")]
        [InlineData(new byte[0], true)]
        [InlineData(default, false)]
        public async Task ShouldValidateContainsClientInCache(byte[] cacheResult, bool expectedResult)
        {
            // arrange
            _cacheMock.Setup(lnq => lnq.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(cacheResult);
            
            // act
            var result = await _clientSseStorage.ContainsAsync(ClientId);

            // assert
            result.Should().Be(expectedResult);
            _cacheMock.Verify(lnq => lnq.GetAsync(GetCacheKey(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Deve atualizar cliente no cache")]
        public async Task ShouldUpdateClientInCache()
        {
            // arrange - act
            await _clientSseStorage.UpdateAsync(ClientId);
            
            // assert
            ValidateCacheSetAsync();
        }

        [Fact(DisplayName = "Deve remover cliente do cache")]
        public async Task ShouldClientFromCache()
        {
            // arrange - act
            await _clientSseStorage.RemoveAsync(ClientId);
            
            // assert
            _cacheMock.Verify(lnq => lnq.RemoveAsync(GetCacheKey(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private void ValidateCacheSetAsync()
        {
            _cacheMock.Verify(
                lnq => lnq.SetAsync(
                    GetCacheKey(),
                    It.IsAny<byte[]>(),
                    It.Is<DistributedCacheEntryOptions>(t => ValidateDistributedCacheEntryOptions(t)),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        private static bool ValidateDistributedCacheEntryOptions(DistributedCacheEntryOptions options)
        {
            var cacheExpirationDate = options.AbsoluteExpiration!.Value;
            var date = new DateTime(cacheExpirationDate.Year, cacheExpirationDate.Month, cacheExpirationDate.Day, cacheExpirationDate.Hour, cacheExpirationDate.Minute, 0);

            var validationDate = DateTime.Now.AddMinutes(MaxTimeCacheInMinutes); 
            var expectedDate = new DateTime(validationDate.Year, validationDate.Month, validationDate.Day, validationDate.Hour, validationDate.Minute, 0);
            date.Should().Be(expectedDate);
            return true;
        }
    }
}