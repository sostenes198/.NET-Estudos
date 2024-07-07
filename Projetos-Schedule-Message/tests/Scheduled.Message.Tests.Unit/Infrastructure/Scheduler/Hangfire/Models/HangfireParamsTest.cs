using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;
using Scheduled.Message.Tests.Base.Utils;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Scheduled.Message.Tests.Unit.Infrastructure.Scheduler.Hangfire.Models;

public class HangfireParamsTest
{
    public class ParamUnitTest
    {
        public required string Prop1 { get; set; }
        public required string Prop2 { get; set; }
        public required string Prop3 { get; set; }
    }

    [Fact]
    public void Should_Validate_Hangfire_Params()
    {
        // Arrange
        var date = new DateTime();
        var id = ObjectId.GenerateNewId();

        var expectedResult = new HangfireParams<ParamUnitTest>
        {
            Id = id,
            ExpireAt = date,
            Input = new ParamUnitTest
            {
                Prop1 = "PROP_1",
                Prop2 = "PROP_2",
                Prop3 = "PROP_3",
            }
        };

        // Act
        var result = new HangfireParams<ParamUnitTest>
        {
            Id = id,
            ExpireAt = date,
            Input = new ParamUnitTest
            {
                Prop1 = "PROP_1",
                Prop2 = "PROP_2",
                Prop3 = "PROP_3",
            }
        };

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Voll_Process_Schedule_Model()
    {
        // Arrange - Act - Assert
        typeof(HangfireParams<ParamUnitTest>).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Id", Type = typeof(ObjectId) },
            new() { Name = "ExpireAt", Type = typeof(DateTime?) },
            new() { Name = "Input", Type = typeof(ParamUnitTest) }
        });

        typeof(HangfireParams<ParamUnitTest>).ValidateAttribute(new List<AttributeProperty<BsonRepresentationAttribute>>
        {
            new()
            {
                Property = nameof(HangfireParams<ParamUnitTest>.Id),
                ValidateAttribute = attr => { attr.Representation.Should().Be(BsonType.ObjectId); }
            },
        });
    }
}