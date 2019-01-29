using System.Threading.Tasks;

namespace Estudos.SSE.Core.ClientsStorages.Contracts
{
    public interface IClientSseStorage
    {
        Task AddAsync(string clientId);

        Task<bool> ContainsAsync(string clientId);

        Task UpdateAsync(string clientId);
    
        Task RemoveAsync(string clientId);
    }
}