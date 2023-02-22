using DocumentManager.Core.Options;
using DocumentManager.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Storage.Net;

namespace DocumentManager.Core;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.Configure<DocumentStorageOptions>(options => options.BlobStorageFactory = StorageFactory.Blobs.InMemory);

        return services
           .AddSingleton<ISystemClock, SystemClock>()
           .AddSingleton<IFileStorage, FileStorage>()
           .AddScoped<IDocumentService, DocumentService>();
    }
}