using MongoDB.Driver;
using Moq;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;

// ReSharper disable VirtualMemberCallInConstructor

namespace Scheduled.Message.Tests.Unit.Infrastructure.Databases.Mongo.Databases.Hangfire;

public class AppMongoDatabaseHangfireTest : AppMongoDatabaseBaseTest
{
    public override Mock<IMongoClient> MongoClient { get; }
    public override IAppMongoDatabase AppMongoDatabase { get; }
    public override IList<string> Collections { get; }

    public AppMongoDatabaseHangfireTest()
    {
        MongoClient = new Mock<IMongoClient>();
        Collections = new List<string>
        {
            "COLLECTION_1",
            "COLLECTION_2",
        };

        AppMongoDatabase = new AppMongoDatabaseHangfire(
            MongoClient.Object,
            new MongoUrlBuilder(),
            Collections);
    }
}