using DocumentManager.Core.Services;
using DocumentManager.Persistence.HostedServices;
using DocumentManager.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentManager.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainPersistence(this IServiceCollection services, string connectionString)
    {
        var migrationsAssemblyName = typeof(SqliteDocumentDbContextFactory).Assembly.GetName().Name;

        return services
           .AddPooledDbContextFactory<DocumentDbContext>(x => x.UseSqlite(connectionString, db => db.MigrationsAssembly(migrationsAssemblyName)))
           .AddSingleton<IDocumentStore, EFCoreDocumentStore>()
           .AddSingleton<IDocumentTypeStore, EFCoreDocumentTypeStore>()
           .AddHostedService<RunMigrations>();
    }
}