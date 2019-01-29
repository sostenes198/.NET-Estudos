using System.Threading.Tasks;
using Estudos.SSE.Core.Authorizations.Contracts;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Core.Authorizations
{
    internal sealed class EmptyAuthorizationSse : IAuthorizationSse
    {
        public Task<bool> AuthorizeAsync(HttpContext context) => Task.FromResult(true);
    }
}