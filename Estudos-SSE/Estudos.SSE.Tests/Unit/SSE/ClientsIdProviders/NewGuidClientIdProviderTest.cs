using Estudos.SSE.Core.ClientsIdProviders;
using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.ClientsIdProviders
{
    public class NewGuidClientIdProviderTest
    {
        private readonly IClientIdProvider _clientIdProvider;

        public NewGuidClientIdProviderTest()
        {
            _clientIdProvider = new NewGuidClientIdProvider();
        }

        [Fact(DisplayName = "Deve gerar cliente id")]
        public async Task ShouldAcquireClientId()
        {
            // arrange - act
            var result = await _clientIdProvider.AcquireClientIdAsync(default!);
            
            // assert
            result.Should().NotBeNullOrEmpty();
            Guid.TryParse(result, out _).Should().BeTrue();
        }
    }
}