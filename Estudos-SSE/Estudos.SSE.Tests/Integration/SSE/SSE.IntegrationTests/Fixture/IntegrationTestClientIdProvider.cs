using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Tests.Integration.SSE.SSE.IntegrationTests.Fixture
{
    public class IntegrationTestClientIdProvider : IClientIdProvider
    {
        public Task<string> AcquireClientIdAsync(HttpContext context)
        {
            var clientId = context.Request.Headers["ClientId"];
            return Task.FromResult(clientId.ToString());
        }
    }
}