using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Core.ClientsIdProviders.Contracts
{
    public interface IClientIdProvider
    {
        Task<string> AcquireClientIdAsync(HttpContext context);
    }
}