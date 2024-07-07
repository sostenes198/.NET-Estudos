using FluentAssertions;
using MongoDB.Driver;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

// ReSharper disable VirtualMemberCallInConstructor

namespace Scheduled.Message.Tests.Integration.Infrastructure.Databases.Mongo.Clients;

public abstract class MongoDatabaseClientBaseTest<TDatabase, TMongoDocument> : BaseTest, IAsyncLifetime
    where TDatabase : IAppMongoDatabase
    where TMongoDocument : MongoDocument
{
    protected abstract string CollectionName { get; }
    protected abstract IAppMongoDatabase AppMongoDatabase { get; }
    protected abstract IMongoDatabaseClient<TDatabase> MongoDatabaseClient { get; }
    protected abstract TMongoDocument[] Documents { get; }

    protected abstract TMongoDocument CreateDocumentTest { get; }
    protected abstract FilterDefinition<TMongoDocument> FilterDefinitionCreateDocumentTest { get; }

    protected abstract TMongoDocument GetDocumentTest { get; }
    protected abstract FilterDefinition<TMongoDocument> FilterDefinitionGetDocumentTest { get; }

    private IMongoDatabase _mongoDatabase = null!;

    private IMongoCollection<TMongoDocument> Collection => _mongoDatabase.GetCollection<TMongoDocument>(CollectionName);

    public async Task InitializeAsync()
    {
        AppMongoDatabase.GetCollections().Add(CollectionName);
        _mongoDatabase = AppMongoDatabase.GetDatabase();

        await _mongoDatabase.CreateCollectionAsync(CollectionName);
        await Collection.InsertManyAsync(Documents);
    }

    public async Task DisposeAsync()
    {
        await _mongoDatabase.DropCollectionAsync(CollectionName);
    }

    [Fact]
    public async Task Should_Create_Index_Successfully()
    {
        // Arrange
        var ttlIndex = new IndexKeysDefinitionBuilder<MongoDocument>().Ascending(lnq => lnq.ExpireAt);
        var options = new CreateIndexOptions { ExpireAfter = TimeSpan.Zero };
        var indexModel = new CreateIndexModel<MongoDocument>(ttlIndex, options);

        // Act
        await MongoDatabaseClient.CreateIndexAsync(CollectionName, indexModel, _ => false, default);

        // Assert
        var bsonDocuments =
            (await Collection.Indexes.ListAsync()).ToList()
            ?? [];

        bsonDocuments.Any(lnq =>
        {
            var indexKeys = lnq["key"].AsBsonDocument;
            return indexKeys.Contains(nameof(MongoDocument.ExpireAt)) && lnq["expireAfterSeconds"].ToInt64() == 0;
        }).Should().BeTrue();
    }

    [Fact]
    public async Task Should_Create_Document_Successfully()
    {
        // Arrange - Act
        await MongoDatabaseClient.CreateAsync(CollectionName, CreateDocumentTest, default);

        // Assert
        var result = await Collection.FindSync(FilterDefinitionCreateDocumentTest).SingleAsync();

        result.Should().BeEquivalentTo(CreateDocumentTest);
    }

    [Fact]
    public async Task Should_List_Documents_Successfully()
    {
        // Arrange - Act
        var result = await MongoDatabaseClient.ListAsync(CollectionName,
            new ExpressionFilterDefinition<TMongoDocument>(_ => true), default);

        // Assert
        result.Should().BeEquivalentTo(Documents);
    }

    [Fact]
    public async Task Should_Get_Document_Successfully()
    {
        // Arrange - Act
        var result = await MongoDatabaseClient.GetAsync(CollectionName,
            FilterDefinitionGetDocumentTest, default);

        // Assert
        result.Should().BeEquivalentTo(GetDocumentTest);
    }
}