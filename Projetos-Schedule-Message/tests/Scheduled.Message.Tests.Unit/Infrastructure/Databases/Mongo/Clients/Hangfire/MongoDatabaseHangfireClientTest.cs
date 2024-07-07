using MongoDB.Driver;
using Moq;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients.Hangfire;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Databases.Mongo.Clients.Hangfire;

public class MongoDatabaseHangfireClientTest : MongoDatabaseClientBaseTest<AppMongoDatabaseHangfire>
{
    protected override (Mock<IMongoCollection<MongoDocument>> mockMongoCollection, Mock<IMongoDatabase>
        mockMongoDatabase, IMongoDatabaseClient<AppMongoDatabaseHangfire> mongoDatabaseClient) Initialize()
    {
        var mongoCollectionMock = new Mock<IMongoCollection<MongoDocument>>(MockBehavior.Strict);

        var mongoDatabaseMock = new Mock<IMongoDatabase>(MockBehavior.Strict);
        mongoDatabaseMock.Setup(lnq =>
                lnq.GetCollection<MongoDocument>(CollectionName, It.IsAny<MongoCollectionSettings>()))
            .Returns(mongoCollectionMock.Object);

        var mongoClientMock = new Mock<IMongoClient>(MockBehavior.Strict);
        mongoClientMock.Setup(lnq => lnq.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
            .Returns(mongoDatabaseMock.Object);

        var mongoUrlBuilder = new MongoUrlBuilder();

        IList<string> collections = new List<string> { CollectionName };

        var appMongoDatabaseHangfire =
            new AppMongoDatabaseHangfire(mongoClientMock.Object, mongoUrlBuilder, collections);

        var mongoDatabaseClient = new MongoDatabaseHangfireClient(appMongoDatabaseHangfire);

        return (mongoCollectionMock, mongoDatabaseMock, mongoDatabaseClient);
    }
}