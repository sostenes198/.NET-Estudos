using MongoDB.Bson;
using MongoDB.Driver;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Clients;

public interface IMongoDatabaseClient<TDatabase>
    where TDatabase : IAppMongoDatabase
{
    Task CreateIndexAsync<TDocument>(string collectionName,
        CreateIndexModel<TDocument> index,
        Func<BsonDocument, bool> verifyIfIndexExist,
        CreateOneIndexOptions? options,
        CancellationToken token = default) where TDocument : MongoDocument;

    Task CreateAsync<TDocument>(string collectionName, TDocument document, InsertOneOptions? options,
        CancellationToken token = default)
        where TDocument : MongoDocument;

    Task<IList<TDocument>> ListAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter,
        FindOptions<TDocument>? options,
        CancellationToken token = default)
        where TDocument : MongoDocument;

    Task<TDocument?> GetAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter,
        FindOptions<TDocument>? options,
        CancellationToken token = default)
        where TDocument : MongoDocument;
}