using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Databases.Mongo.Clients;

public abstract class MongoDatabaseClientBaseTest<TDatabase> : IDisposable
    where TDatabase : IAppMongoDatabase
{
    protected const string CollectionName = $"{nameof(MongoDatabaseClientBaseTest<TDatabase>)}.UnitTest";

    private readonly Mock<IMongoCollection<MongoDocument>> _mongoCollectionMock;
    private readonly Mock<IMongoDatabase> _mongoDatabaseMock;
    private readonly IMongoDatabaseClient<TDatabase> _mongoDatabaseClient;

    protected abstract (Mock<IMongoCollection<MongoDocument>> mockMongoCollection, Mock<IMongoDatabase>
        mockMongoDatabase,
        IMongoDatabaseClient<TDatabase> mongoDatabaseClient)
        Initialize();

    public MongoDatabaseClientBaseTest()
    {
        var initializeObject = Initialize();

        _mongoCollectionMock = initializeObject.mockMongoCollection;

        _mongoDatabaseMock = initializeObject.mockMongoDatabase;
        _mongoDatabaseMock.Setup(lnq =>
                lnq.GetCollection<MongoDocument>(CollectionName, It.IsAny<MongoCollectionSettings>()))
            .Returns(_mongoCollectionMock.Object);

        var mongoClientMock = new Mock<IMongoClient>(MockBehavior.Strict);
        mongoClientMock.Setup(lnq => lnq.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
            .Returns(_mongoDatabaseMock.Object);

        _mongoDatabaseClient = initializeObject.mongoDatabaseClient;
    }

    #region CreateIndex

    [Fact]
    public async Task Should_Create_Index_Successfully()
    {
        // arrange
        var indexModel =
            new CreateIndexModel<MongoDocument>(new BsonDocumentIndexKeysDefinition<MongoDocument>(new BsonDocument()));
        var cancellationToken = CancellationToken.None;

        var mongoIndexManager = new Mock<IMongoIndexManager<MongoDocument>>(MockBehavior.Strict);
        mongoIndexManager.Setup(lnq => lnq.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<IAsyncCursor<BsonDocument>>().Object);
        mongoIndexManager.Setup(lnq => lnq.CreateOneAsync(It.IsAny<CreateIndexModel<MongoDocument>>(),
            It.IsAny<CreateOneIndexOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync("");

        var asyncCursor = new Mock<IAsyncCursor<string>>();
        asyncCursor
            .SetupSequence(lnq => lnq.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(true)
            .Returns(true)
            .Returns(false);
        asyncCursor.SetupSequence(lnq => lnq.Current)
            .Returns(new[] { "1" })
            .Returns(new[] { "2" })
            .Returns(new[] { CollectionName });

        _mongoDatabaseMock.Setup(lnq => lnq.ListCollectionNamesAsync(
            It.IsAny<ListCollectionNamesOptions>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(asyncCursor.Object);

        _mongoCollectionMock.SetupGet(lnq => lnq.Indexes).Returns(mongoIndexManager.Object);

        // act
        await _mongoDatabaseClient.CreateIndexAsync(CollectionName,
            indexModel,
            _ => false,
            default,
            cancellationToken);

        // assert
        _mongoDatabaseMock.Verify(lnq => lnq
                .ListCollectionNamesAsync(default,
                    cancellationToken),
            Times.Once);

        _mongoDatabaseMock.Verify(lnq => lnq
            .GetCollection<MongoDocument>(CollectionName, default), Times.Once);
        mongoIndexManager.Verify(lnq => lnq.ListAsync(cancellationToken), Times.Once);
        mongoIndexManager.Verify(lnq => lnq
            .CreateOneAsync(MoqAssert.Assert(indexModel), default, cancellationToken), Times.Once);
    }

    [Fact]
    public async Task Should_Create_Index_And_Collection_Successfully_When_Collection_Does_Not_Exist()
    {
        // arrange
        var indexModel =
            new CreateIndexModel<MongoDocument>(new BsonDocumentIndexKeysDefinition<MongoDocument>(new BsonDocument()));
        var token = CancellationToken.None;

        var mongoIndexManager = new Mock<IMongoIndexManager<MongoDocument>>(MockBehavior.Strict);
        mongoIndexManager.Setup(lnq => lnq.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<IAsyncCursor<BsonDocument>>().Object);
        mongoIndexManager.Setup(lnq => lnq.CreateOneAsync(It.IsAny<CreateIndexModel<MongoDocument>>(),
            It.IsAny<CreateOneIndexOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync("");

        var asyncCursor = new Mock<IAsyncCursor<string>>();
        asyncCursor
            .SetupSequence(lnq => lnq.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(true)
            .Returns(false);
        asyncCursor.SetupSequence(lnq => lnq.Current)
            .Returns(new[] { "1" })
            .Returns(new[] { "2" });

        _mongoDatabaseMock.Setup(lnq => lnq.ListCollectionNamesAsync(
            It.IsAny<ListCollectionNamesOptions>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(asyncCursor.Object);

        _mongoDatabaseMock.Setup(lnq => lnq.CreateCollectionAsync(
            It.IsAny<string>(), It.IsAny<CreateCollectionOptions>(), It.IsAny<CancellationToken>()
        )).Returns(Task.CompletedTask);

        _mongoCollectionMock.SetupGet(lnq => lnq.Indexes).Returns(mongoIndexManager.Object);

        // act
        await _mongoDatabaseClient.CreateIndexAsync(CollectionName,
            indexModel,
            _ => false,
            default,
            token);

        // assert
        _mongoDatabaseMock.Verify(lnq => lnq
                .ListCollectionNamesAsync(default,
                    token),
            Times.Once);

        _mongoDatabaseMock.Verify(lnq => lnq
            .GetCollection<MongoDocument>(CollectionName, default), Times.Once);

        _mongoDatabaseMock.Verify(lnq => lnq
            .CreateCollectionAsync(CollectionName, default, token), Times.Once);

        mongoIndexManager.Verify(lnq => lnq.ListAsync(token), Times.Once);

        mongoIndexManager.Verify(lnq => lnq
            .CreateOneAsync(MoqAssert.Assert(indexModel), default, token), Times.Once);
    }

    [Fact]
    public async Task Should_Not_Create_Index_Successfully_When_Index_Exist()
    {
        // arrange
        var indexModel =
            new CreateIndexModel<MongoDocument>(new BsonDocumentIndexKeysDefinition<MongoDocument>(new BsonDocument()));
        var cancellationToken = CancellationToken.None;

        var asyncCursorBsonDocumentMock = new Mock<IAsyncCursor<BsonDocument>>();
        asyncCursorBsonDocumentMock.SetupSequence(lnq => lnq.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(false);
        asyncCursorBsonDocumentMock.SetupSequence(lnq => lnq.Current)
            .Returns(new[] { new BsonDocument() });

        var mongoIndexManager = new Mock<IMongoIndexManager<MongoDocument>>(MockBehavior.Strict);
        mongoIndexManager.Setup(lnq => lnq.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursorBsonDocumentMock.Object);
        mongoIndexManager.Setup(lnq => lnq.CreateOneAsync(It.IsAny<CreateIndexModel<MongoDocument>>(),
            It.IsAny<CreateOneIndexOptions>(), It.IsAny<CancellationToken>())).ReturnsAsync("");

        var asyncCursor = new Mock<IAsyncCursor<string>>();
        asyncCursor
            .SetupSequence(lnq => lnq.MoveNext(It.IsAny<CancellationToken>()))
            .Returns(true)
            .Returns(true)
            .Returns(true)
            .Returns(false);
        asyncCursor.SetupSequence(lnq => lnq.Current)
            .Returns(new[] { "1" })
            .Returns(new[] { "2" })
            .Returns(new[] { CollectionName });

        _mongoDatabaseMock.Setup(lnq => lnq.ListCollectionNamesAsync(
            It.IsAny<ListCollectionNamesOptions>(), It.IsAny<CancellationToken>()
        )).ReturnsAsync(asyncCursor.Object);

        _mongoCollectionMock.SetupGet(lnq => lnq.Indexes).Returns(mongoIndexManager.Object);

        // act
        await _mongoDatabaseClient.CreateIndexAsync(CollectionName,
            indexModel,
            _ => true,
            default,
            cancellationToken);

        // assert
        _mongoDatabaseMock.Verify(lnq => lnq
                .ListCollectionNamesAsync(default,
                    cancellationToken),
            Times.Once);

        _mongoCollectionMock.Verify(lnq => lnq.Indexes, Times.Once);

        _mongoDatabaseMock.Verify(lnq => lnq
            .GetCollection<MongoDocument>(CollectionName, default), Times.Once);
        mongoIndexManager.Verify(lnq => lnq.ListAsync(cancellationToken), Times.Once);
        mongoIndexManager.Verify(lnq => lnq
            .CreateOneAsync(MoqAssert.Assert(indexModel), default, cancellationToken), Times.Never);
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task Should_Create_Document_Successfully()
    {
        // arrange
        var token = CancellationToken.None;
        var document = new MongoDocument();

        _mongoCollectionMock.Setup(lnq => lnq.InsertOneAsync(
            It.IsAny<MongoDocument>(),
            It.IsAny<InsertOneOptions>(),
            It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // act
        await _mongoDatabaseClient.CreateAsync(CollectionName, document, default, token);

        // assert
        _mongoDatabaseMock.Verify(lnq => lnq
            .GetCollection<MongoDocument>(CollectionName, default), Times.Once);
        _mongoCollectionMock.Verify(lnq => lnq.InsertOneAsync(
            MoqAssert.Assert(document),
            default,
            token), Times.Once);
    }

    #endregion

    #region ListAsync

    public record ListAsyncScenario(String Name, List<MongoDocument> Documents, List<MongoDocument> ExpectedResult);

    public static IEnumerable<object[]> ScenariosListAsync()
    {
        yield return
        [
            new ListAsyncScenario("When has documents", [new MongoDocument(), new MongoDocument()],
                [new MongoDocument(), new MongoDocument()])
        ];
        yield return
        [
            new ListAsyncScenario("When empty documents", [],
                [])
        ];
    }

    [Theory]
    [MemberData(nameof(ScenariosListAsync))]
    public async Task Should_List_Documents_Successfully(ListAsyncScenario scenario)
    {
        // arrange
        var token = CancellationToken.None;
        FilterDefinition<MongoDocument> filterDefinition = new ExpressionFilterDefinition<MongoDocument>(_ => true);

        var asyncCursorMock = new Mock<IAsyncCursor<MongoDocument>>();


        var moveNextMock = asyncCursorMock.SetupSequence(lnq => lnq.MoveNext(It.IsAny<CancellationToken>()));
        for (var i = 0; i < scenario.Documents.Count; i++)
        {
            moveNextMock.Returns(true);
        }

        moveNextMock.Returns(false);


        var currentMock = asyncCursorMock.SetupSequence(lnq => lnq.Current);
        scenario.Documents.ForEach(item => currentMock.Returns(new[] { item }));

        _mongoCollectionMock.Setup(lnq => lnq.FindAsync(It.IsAny<FilterDefinition<MongoDocument>>(),
                It.IsAny<FindOptions<MongoDocument>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursorMock.Object);

        var expectedResult = scenario.ExpectedResult;

        // act
        var result = await _mongoDatabaseClient.ListAsync(CollectionName, filterDefinition, default, token);

        // assert
        result.Should().BeEquivalentTo(expectedResult);
        _mongoDatabaseMock.Verify(lnq => lnq
            .GetCollection<MongoDocument>(CollectionName, default), Times.Once);
    }

    #endregion

    #region GetAsync

    public record GetAsyncScenario(String Name, List<MongoDocument> Documents, MongoDocument? ExpectedResult);

    public static IEnumerable<object[]> ScenariosGetAsync()
    {
        yield return
        [
            new GetAsyncScenario("When has document", [new MongoDocument(), new MongoDocument()],
                new MongoDocument())
        ];
        yield return
        [
            new GetAsyncScenario("When empty document", [],
                null)
        ];
    }

    [Theory]
    [MemberData(nameof(ScenariosGetAsync))]
    public async Task Should_Get_Document_Successfully(GetAsyncScenario scenario)
    {
        // arrange
        var token = CancellationToken.None;
        FilterDefinition<MongoDocument> filterDefinition = new ExpressionFilterDefinition<MongoDocument>(_ => true);

        var expectedResult = scenario.ExpectedResult;

        var asyncCursorMock = new Mock<IAsyncCursor<MongoDocument>>();

        var moveNextMock = asyncCursorMock.SetupSequence(lnq => lnq.MoveNext(It.IsAny<CancellationToken>()));
        for (var i = 0; i < scenario.Documents.Count; i++)
        {
            moveNextMock.Returns(true);
        }

        moveNextMock.Returns(false);


        var currentMock = asyncCursorMock.SetupSequence(lnq => lnq.Current);
        scenario.Documents.ForEach(item => currentMock.Returns(new[] { item }));

        _mongoCollectionMock.Setup(lnq => lnq.FindAsync(It.IsAny<FilterDefinition<MongoDocument>>(),
                It.IsAny<FindOptions<MongoDocument>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursorMock.Object);

        _mongoCollectionMock.Setup(lnq => lnq.FindAsync(It.IsAny<FilterDefinition<MongoDocument>>(),
                It.IsAny<FindOptions<MongoDocument>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(asyncCursorMock.Object);

        // act
        var result =
            await _mongoDatabaseClient.GetAsync<MongoDocument>(CollectionName, filterDefinition, default, token);

        // assert
        result.Should().BeEquivalentTo(expectedResult);
        _mongoDatabaseMock.Verify(lnq => lnq
            .GetCollection<MongoDocument>(CollectionName, default), Times.Once);
    }

    #endregion


    public void Dispose()
    {
    }
}