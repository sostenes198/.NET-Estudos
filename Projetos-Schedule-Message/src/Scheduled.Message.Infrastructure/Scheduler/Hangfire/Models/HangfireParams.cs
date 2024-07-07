using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;

public record HangfireParams<TInput> : MongoDocument
    where TInput : class
{
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; } = ObjectId.Empty;

    public TInput? Input { get; set; }
}