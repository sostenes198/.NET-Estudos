using FluentAssertions;
using MongoDB.Bson.Serialization.Attributes;
using Scheduled.Message.Infrastructure.Databases.Mongo.Models;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Databases.Mongo.Models;

public class MongoDocumentTest
{
    [Fact]
    public void Should_Validate_Mongo_Document()
    {
        // Arrange
        var date = new DateTime();
        var expectedResult = new MongoDocument
        {
            ExpireAt = date
        };

        // Act
        var result = new MongoDocument
        {
            ExpireAt = date
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Mongo_Document()
    {
        // Arrange - Act - Assert
        typeof(MongoDocument).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "ExpireAt", Type = typeof(DateTime?) }
        });

        typeof(MongoDocument).ValidateAttribute(new List<AttributeProperty<BsonElementAttribute>>
        {
            new()
            {
                Property = nameof(MongoDocument.ExpireAt),
                ValidateAttribute = _ => { }
            },
        });

        typeof(MongoDocument).ValidateAttribute(new List<AttributeProperty<BsonDateTimeOptionsAttribute>>
        {
            new()
            {
                Property = nameof(MongoDocument.ExpireAt),
                ValidateAttribute = attr => { attr.Kind.Should().Be(DateTimeKind.Utc); }
            },
        });
    }
}