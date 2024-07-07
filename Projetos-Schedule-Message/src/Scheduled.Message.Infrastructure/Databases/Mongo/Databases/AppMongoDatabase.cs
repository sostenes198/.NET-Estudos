using MongoDB.Driver;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Databases;

public abstract class AppMongoDatabase(
    IMongoClient mongoClient,
    MongoUrlBuilder mongoUrlBuilder,
    IList<string> collections) : IAppMongoDatabase
{
    private readonly IList<string> _collections = new HashSet<string>(collections).ToList();

    public IMongoClient GetMongoClient()
    {
        return mongoClient;
    }

    public MongoUrlBuilder GetMongoUrlBuilder()
    {
        return mongoUrlBuilder;
    }

    public IMongoDatabase GetDatabase()
    {
        return mongoClient.GetDatabase(mongoUrlBuilder.DatabaseName);
    }

    public IList<string> GetCollections()
    {
        return _collections;
    }

    public string GetValidCollection(string collectionName)
    {
        if (!_collections.Contains(collectionName))
        {
            throw new Exception(
                $"Not found {collectionName} collection configured for {nameof(AppMongoDatabase)} database");
        }

        return collectionName;
    }
}