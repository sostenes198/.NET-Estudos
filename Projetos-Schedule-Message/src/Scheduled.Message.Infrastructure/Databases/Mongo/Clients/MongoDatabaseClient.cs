using MongoDB.Bson;
using MongoDB.Driver;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Clients;

public abstract class MongoDatabaseClient<TDatabase>(TDatabase appMongoDatabase)
    : IMongoDatabaseClient<TDatabase>
    where TDatabase : IAppMongoDatabase
{
    private readonly IMongoDatabase _mongoDatabase = appMongoDatabase.GetDatabase();

    public async Task CreateIndexAsync<TDocument>(string collectionName,
        CreateIndexModel<TDocument> index,
        Func<BsonDocument, bool> verifyIfIndexExist,
        CreateOneIndexOptions? options,
        CancellationToken token = default) where TDocument : MongoDocument
    {
        var collections =
            (await _mongoDatabase.ListCollectionNamesAsync(cancellationToken: token))
            .ToList(cancellationToken: token) ?? [];

        if (!collections.Contains(collectionName))
            await _mongoDatabase.CreateCollectionAsync(collectionName, cancellationToken: token);

        var collection = Collection<TDocument>(collectionName);

        var bsonDocuments =
            (await collection.Indexes.ListAsync(token)).ToList(cancellationToken: token)
            ?? [];

        var existIndex = bsonDocuments.Any(verifyIfIndexExist);

        if (existIndex is false)
        {
            await collection.Indexes.CreateOneAsync(index, options, token);
        }
    }

    public Task CreateAsync<TDocument>(string collectionName, TDocument document, InsertOneOptions? options,
        CancellationToken token = default) where TDocument : MongoDocument
    {
        var collection = Collection<TDocument>(collectionName);
        return collection.InsertOneAsync(document, options, token);
    }

    public async Task<IList<TDocument>> ListAsync<TDocument>(string collectionName, FilterDefinition<TDocument> filter,
        FindOptions<TDocument>? options,
        CancellationToken token = default) where TDocument : MongoDocument
    {
        var collection = Collection<TDocument>(collectionName);
        var cursor = await collection.FindAsync(filter, options, token);
        return cursor.ToList(cancellationToken: token);
    }

    public async Task<TDocument?> GetAsync<TDocument>(
        string collectionName,
        FilterDefinition<TDocument> filter,
        FindOptions<TDocument>? options,
        CancellationToken token = default) where TDocument : MongoDocument
    {
        var collection = Collection<TDocument>(collectionName);
        return (await collection.FindAsync(filter, options, token)).FirstOrDefault();
    }

    private IMongoCollection<TDocument> Collection<TDocument>(string collectionName) =>
        _mongoDatabase.GetCollection<TDocument>(appMongoDatabase.GetValidCollection(collectionName));
}