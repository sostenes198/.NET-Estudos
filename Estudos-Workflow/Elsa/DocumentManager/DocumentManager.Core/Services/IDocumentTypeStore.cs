using DocumentManager.Core.Models;

namespace DocumentManager.Core.Services;

public interface IDocumentTypeStore
{
    Task<IEnumerable<DocumentType>> ListAsync(CancellationToken cancellationToken = default);
    Task<DocumentType?> GetAsync(string id, CancellationToken cancellationToken = default);
}