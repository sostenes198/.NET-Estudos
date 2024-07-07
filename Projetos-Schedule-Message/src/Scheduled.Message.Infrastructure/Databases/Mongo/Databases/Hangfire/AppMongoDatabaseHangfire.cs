using MongoDB.Driver;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;

public class AppMongoDatabaseHangfire(
    IMongoClient mongoClient,
    MongoUrlBuilder mongoUrlBuilder,
    IList<string> collections) : AppMongoDatabase(mongoClient, mongoUrlBuilder, collections);