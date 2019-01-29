using System.Threading.Tasks;

namespace Estudos.Checklist.ConfiguracoesPc.Domain.Interfaces
{
    public interface IScan
    {
        Task<IScanResult> ScanAsync();
    }
}