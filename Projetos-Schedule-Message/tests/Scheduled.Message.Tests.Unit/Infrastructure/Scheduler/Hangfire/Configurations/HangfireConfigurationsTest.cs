using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Scheduler.Hangfire.Configurations;

public class HangfireConfigurationsTest
{
    [Fact]
    public void Should_Validate_Section_Path()
    {
        // Arrange - Act - Assert
        HangfireConfigurations.Section.Should().BeEquivalentTo("Hangfire");
    }

    [Fact]
    public void Should_Validate_Hangfire_Configurations()
    {
        // Arrange
        var expectedResult = new HangfireConfigurations
        {
            TtlHangfireDocumentInDays = 10
        };

        // Act
        var hangfireConfigurations = HangfireConfigurationsFaker.Create();

        // Assert
        hangfireConfigurations.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Hangfire_Configurations()
    {
        // Arrange - Act - Assert
        typeof(HangfireConfigurations).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Section", Type = typeof(string) },
            new() { Name = "TtlHangfireDocumentInDays", Type = typeof(int) },
        });

        typeof(HangfireConfigurations).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(HangfireConfigurations.TtlHangfireDocumentInDays),
                ValidateAttribute = attr => { attr.AllowEmptyStrings.Should().BeFalse(); }
            }
        });
    }
}