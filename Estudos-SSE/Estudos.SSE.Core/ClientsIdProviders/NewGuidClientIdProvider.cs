using System;
using System.Threading.Tasks;
using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Core.ClientsIdProviders
{
    internal sealed class NewGuidClientIdProvider : IClientIdProvider
    {
        public Task<string> AcquireClientIdAsync(HttpContext context) => Task.FromResult(Guid.NewGuid().ToString());
    }
}