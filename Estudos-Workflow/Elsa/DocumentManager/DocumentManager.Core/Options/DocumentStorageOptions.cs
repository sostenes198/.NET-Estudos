using Storage.Net;
using Storage.Net.Blobs;

namespace DocumentManager.Core.Options;

public class DocumentStorageOptions
{
    public Func<IBlobStorage> BlobStorageFactory { get; set; } = StorageFactory.Blobs.InMemory;
}