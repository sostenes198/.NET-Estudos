using DocumentManager.WorkFlows.Activities;
using DocumentManager.WorkFlows.Handlers;
using DocumentManager.WorkFlows.Scripting.JavaScript;
using Elsa;
using Elsa.Persistence.EntityFramework.Core.Extensions;
using Elsa.Providers.Workflows;
using Elsa.Server.Hangfire.Extensions;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Storage.Net;

namespace DocumentManager.WorkFlows;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkflowServices(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
    {
        services.AddNotificationHandlersFrom<StartDocumentWorkflows>();
        
        var currentAssemblyPath = Path.GetDirectoryName(typeof(ServiceCollectionExtensions).Assembly.Location)!;

        services.Configure<BlobStorageWorkflowProviderOptions>(options => options.BlobStorageFactory = () => StorageFactory.Blobs.DirectoryFiles(Path.Combine(currentAssemblyPath, "Workflows")));

        services.AddJavaScriptTypeDefinitionProvider<CustomTypeDefinitionProvider>();

        services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

        return services.AddElsa(configureDb);
    }

    private static IServiceCollection AddElsa(this IServiceCollection services, Action<DbContextOptionsBuilder> configureDb)
    {
     services
     .AddElsa(elsa => elsa

               // Use EF Core's SQLite provider to store workflow instances and bookmarks.
              .UseEntityFrameworkPersistence(configureDb)

               // Ue Console activities for testing & demo purposes.
              .AddConsoleActivities()

               // Use Hangfire to dispatch workflows from.
              .UseHangfireDispatchers()

               // Configure Email activities.
              .AddEmailActivities()

               // Configure HTTP activities.
              .AddHttpActivities()

               // Custom Activities
              .AddActivitiesFrom<GetDocument>()
              .AddActivitiesFrom<ArchiveDocument>()
              .AddActivitiesFrom<ZipFile>()
              .AddActivitiesFrom<UpdateBlockchain>());

     return services;
    }
}