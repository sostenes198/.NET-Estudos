using DocumentManager.Core.Models;

namespace DocumentManager.Core.Services;

public interface IDocumentService
{
    Task<Document> SaveDocumentAsync(string fileName, Stream data, string documentTypeId, CancellationToken cancellationToken = default);
}