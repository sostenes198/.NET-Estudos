using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

namespace Scheduled.Message.Api.Bootstrappers;

[ExcludeFromCodeCoverage]
public static class MongoDbInitialize
{
    public static async Task InitializeAsync(IServiceProvider provider, CancellationToken token)
    {
        var mongoConfigurations = provider.GetRequiredService<IOptions<MongoConfigurations>>().Value;

        if (mongoConfigurations.CreateIndex.Enabled is false)
            return;

        var appMongoDatabases = provider.GetServices<IAppMongoDatabase>();
        foreach (var appMongoDatabase in appMongoDatabases)
        {
            var collectionsName = appMongoDatabase.GetCollections();
            var mongoDatabaseClient = provider.GetRequiredService(typeof(IMongoDatabaseClient<>)
                .MakeGenericType(appMongoDatabase.GetType()));

            var methodInfo = GetMethodInfo(mongoDatabaseClient);

            foreach (var collectionName in collectionsName)
            {
                await CreateTTlIndex(mongoDatabaseClient, collectionName, methodInfo, mongoConfigurations, token);
            }
        }
    }

    private static MethodInfo GetMethodInfo(object mongoDatabaseClient)
    {
        return
            mongoDatabaseClient.GetType().GetMethod("CreateIndexAsync")?.MakeGenericMethod(typeof(MongoDocument))
            ?? throw new Exception("Method CreateIndex is required");
    }

    private static async Task CreateTTlIndex(
        object mongoDatabaseClient,
        string collectionName,
        MethodInfo createIndexMethod,
        MongoConfigurations mongoConfigurations,
        CancellationToken token)
    {
        if (mongoConfigurations.CreateIndex.TTL.Enabled is false)
            return;

        await (Task)createIndexMethod.Invoke(mongoDatabaseClient, [
            collectionName,
            CreateIndex(),
            (Func<BsonDocument, bool>)VerifyIfIndexExist,
            default,
            token
        ])!;
        
        return;

        static CreateIndexModel<MongoDocument> CreateIndex()
        {
            var ttlIndex = new IndexKeysDefinitionBuilder<MongoDocument>().Ascending(lnq => lnq.ExpireAt);
            var options = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };
            return new CreateIndexModel<MongoDocument>(ttlIndex, options);
        }

        static bool VerifyIfIndexExist(BsonDocument lnq)
        {
            var indexKeys = lnq["key"].AsBsonDocument;
            return indexKeys.Contains(nameof(MongoDocument.ExpireAt)) && lnq["expireAfterSeconds"].ToInt64() == 0;
        }
    }
}