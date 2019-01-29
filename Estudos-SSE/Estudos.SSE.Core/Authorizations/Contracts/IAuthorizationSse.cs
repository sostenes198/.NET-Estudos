using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Core.Authorizations.Contracts
{
    public interface IAuthorizationSse
    {
        Task<bool> AuthorizeAsync(HttpContext context);
    }
}