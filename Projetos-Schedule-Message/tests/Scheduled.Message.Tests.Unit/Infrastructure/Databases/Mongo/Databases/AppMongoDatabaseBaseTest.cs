using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Databases.Mongo.Databases;

public abstract class AppMongoDatabaseBaseTest : IAsyncLifetime
{
    public abstract Mock<IMongoClient> MongoClient { get; }
    public abstract IAppMongoDatabase AppMongoDatabase { get; }
    public abstract IList<string> Collections { get; }

    public Task InitializeAsync()
    {
        MongoClient.Setup(lnq => lnq.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
            .Returns(new Mock<IMongoDatabase>().Object);

        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    [Fact]
    public void Should_Get_Mongo_Client()
    {
        // Arrange - Act
        var mongoClient = AppMongoDatabase.GetMongoClient();

        // Assert
        mongoClient.Should().NotBeNull();
    }

    [Fact]
    public void Should_Get_Mongo_Url_Builder()
    {
        // Arrange - Act
        var mongoUrlBuilder = AppMongoDatabase.GetMongoUrlBuilder();

        // Assert
        mongoUrlBuilder.Should().NotBeNull();
    }

    [Fact]
    public void Should_Get_Database()
    {
        // Arrange - Act
        var mongoDatabase = AppMongoDatabase.GetDatabase();

        // Assert
        mongoDatabase.Should().NotBeNull();
    }

    [Fact]
    public void Should_Get_Collections()
    {
        // Arrange - Act
        var collections = AppMongoDatabase.GetCollections();

        // Assert
        collections.Should().BeEquivalentTo(Collections);
    }

    [Fact]
    public void Should_Get_Valid_Collection()
    {
        // Arrange - Act
        var result = AppMongoDatabase.GetValidCollection(Collections[0]);

        // Assert
        result.Should().BeEquivalentTo(Collections[0]);
    }

    [Fact]
    public void Should_Thrown_Exception_When_Collection_Does_Not_Contain_Collection()
    {
        // Arrange
        var collectionName = "NOT_FOUND";

        // Act
        Action act = () => AppMongoDatabase.GetValidCollection(collectionName);

        // Assert
        act.Should().Throw<Exception>()
            .WithMessage($"Not found {collectionName} collection configured for {nameof(AppMongoDatabase)} database");
    }
}