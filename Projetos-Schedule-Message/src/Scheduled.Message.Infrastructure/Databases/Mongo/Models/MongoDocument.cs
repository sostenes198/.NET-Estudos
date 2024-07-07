using MongoDB.Bson.Serialization.Attributes;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Models;

public record MongoDocument
{
    [BsonElement]
    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime? ExpireAt { get; set; }
}