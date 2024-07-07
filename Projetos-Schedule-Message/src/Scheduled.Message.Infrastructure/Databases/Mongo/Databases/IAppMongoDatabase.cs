using MongoDB.Driver;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Databases;

public interface IAppMongoDatabase
{
    public IMongoClient GetMongoClient();

    public MongoUrlBuilder GetMongoUrlBuilder();

    public IMongoDatabase GetDatabase();

    public IList<string> GetCollections();

    public string GetValidCollection(string collectionName);
}