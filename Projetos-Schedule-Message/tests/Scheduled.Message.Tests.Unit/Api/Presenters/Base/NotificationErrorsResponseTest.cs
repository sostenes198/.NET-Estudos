using System.Text.Json.Serialization;
using FluentAssertions;
using Scheduled.Message.Api.Presenters.Base;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Api.Presenters.Base;

public class NotificationErrorsResponseTest
{
    [Fact]
    public void Should_Validate_Notification_Error_Response()
    {
        // Arrange
        var expectedResult = new NotificationErrorsResponse(
            new Dictionary<string, string[]>
            {
                { "PROP_1", ["ERROR_1", "ERROR_2"] },
                { "PROP_2", ["ERROR_1"] }
            });

        // Act
        var result = NotificationErrorsResponseFaker.Create();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Notification_Error_Response()
    {
        // Arrange - Act - Assert
        typeof(NotificationErrorsResponse).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Errors", Type = typeof(IDictionary<string, string[]>) }
        });

        typeof(NotificationErrorsResponse).ValidateAttribute(new List<AttributeProperty<JsonPropertyNameAttribute>>
        {
            new()
            {
                Property = nameof(NotificationErrorsResponse.Errors),
                ValidateAttribute = attr => { attr.Name.Should().BeEquivalentTo("errors"); }
            }
        });
    }
}