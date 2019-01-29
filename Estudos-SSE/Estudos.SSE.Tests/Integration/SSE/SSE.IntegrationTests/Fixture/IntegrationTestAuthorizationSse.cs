using Estudos.SSE.Core.Authorizations.Contracts;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Tests.Integration.SSE.SSE.IntegrationTests.Fixture
{
    public class IntegrationTestAuthorizationSse : IAuthorizationSse
    {
        public Task<bool> AuthorizeAsync(HttpContext context)
        {
            return Task.FromResult(context.Request.Headers["Login"] == "IntegrationTest" && context.Request.Headers["Password"] == "123456789");
        }
    }
}