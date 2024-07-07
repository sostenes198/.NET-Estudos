using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

// ReSharper disable VirtualMemberCallInConstructor

namespace Scheduled.Message.Tests.Integration.Infrastructure.Databases.Mongo.Clients.Hangfire;

public record MongoDatabaseHangfireMongoDocumentTest : MongoDocument
{
    public required string Id { get; set; }
    public required string Value { get; set; }
}

public class MongoDatabaseHangfireClientTest : MongoDatabaseClientBaseTest<AppMongoDatabaseHangfire,
    MongoDatabaseHangfireMongoDocumentTest>
{
    protected override string CollectionName { get; }
    protected override IAppMongoDatabase AppMongoDatabase { get; }
    protected override IMongoDatabaseClient<AppMongoDatabaseHangfire> MongoDatabaseClient { get; }
    protected override MongoDatabaseHangfireMongoDocumentTest[] Documents { get; }
    protected override MongoDatabaseHangfireMongoDocumentTest CreateDocumentTest { get; }

    protected override FilterDefinition<MongoDatabaseHangfireMongoDocumentTest> FilterDefinitionCreateDocumentTest
    {
        get;
    }

    protected override MongoDatabaseHangfireMongoDocumentTest GetDocumentTest { get; }
    protected override FilterDefinition<MongoDatabaseHangfireMongoDocumentTest> FilterDefinitionGetDocumentTest { get; }

    public MongoDatabaseHangfireClientTest()
    {
        CollectionName = "IntegrationTestMongoDatabaseHangfireClientTest";
        AppMongoDatabase = ServiceProvider.GetRequiredService<AppMongoDatabaseHangfire>();
        MongoDatabaseClient = ServiceProvider.GetRequiredService<IMongoDatabaseClient<AppMongoDatabaseHangfire>>();
        Documents =
        [
            new MongoDatabaseHangfireMongoDocumentTest
                { Id = "cbe0d766-d191-4dd7-8089-5dd80033c2bd", Value = "VALUE_1" },
            new MongoDatabaseHangfireMongoDocumentTest
                { Id = "f4a3fac2-2605-464a-b85e-cdf5e97dceff", Value = "VALUE_2" },
            new MongoDatabaseHangfireMongoDocumentTest
                { Id = "3847a0c2-48e9-43af-966c-98096387510b", Value = "VALUE_3" },
            new MongoDatabaseHangfireMongoDocumentTest
                { Id = "23e16443-f774-4c74-be17-e89b496ccb0b", Value = "VALUE_4" },
        ];
        CreateDocumentTest = new MongoDatabaseHangfireMongoDocumentTest
            { Id = "b48f3956-e5dd-4419-b6a7-75b1046f3936", Value = "VALUE_5" };
        FilterDefinitionCreateDocumentTest = new ExpressionFilterDefinition<MongoDatabaseHangfireMongoDocumentTest>(
            lnq => lnq.Id == CreateDocumentTest.Id);

        GetDocumentTest = Documents[1];
        FilterDefinitionGetDocumentTest = new ExpressionFilterDefinition<MongoDatabaseHangfireMongoDocumentTest>(
            lnq => lnq.Id == GetDocumentTest.Id);
    }
}