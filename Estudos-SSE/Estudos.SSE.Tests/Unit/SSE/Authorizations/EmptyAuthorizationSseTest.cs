using Estudos.SSE.Core.Authorizations;
using Estudos.SSE.Core.Authorizations.Contracts;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Authorizations
{
    public class EmptyAuthorizationSseTest
    {
        private readonly IAuthorizationSse _authorizationSse;

        public EmptyAuthorizationSseTest()
        {
            _authorizationSse = new EmptyAuthorizationSse();
        }

        [Fact(DisplayName = "Deve validar authorização")]
        public async Task ShouldValidateAuthorization()
        {
            // arrange - act
            var result = await _authorizationSse.AuthorizeAsync(default!);
            
            // assert
            result.Should().BeTrue();
        }
    }
}