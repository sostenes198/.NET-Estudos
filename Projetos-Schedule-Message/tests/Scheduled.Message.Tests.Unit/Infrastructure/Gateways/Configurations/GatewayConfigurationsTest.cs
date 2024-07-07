using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Scheduled.Message.Infrastructure.Gateways.Configurations;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Gateways.Configurations;

public class GatewayConfigurationsTest
{
    [Fact]
    public void Should_Validate_Section_Path()
    {
        // Arrange - Act - Assert
        GatewayConfigurations.Section.Should().BeEquivalentTo("Gateways");
    }

    [Fact]
    public void Should_Validate_Gateway_Configurations()
    {
        // Arrange
        var expectedResult = new GatewayConfigurations
        {
            VollScheduler = new GatewayConfigurationsVollConfigurations
            {
                BaseUrl = "http://voll-scheduler-unit-test.com"
            }
        };

        // Act
        var result = GatewayConfigurationsFaker.Create();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Mongo_Configurations()
    {
        // Arrange - Act - Assert
        typeof(GatewayConfigurations).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Section", Type = typeof(string) },
            new() { Name = "VollScheduler", Type = typeof(GatewayConfigurationsVollConfigurations) },
        });

        typeof(GatewayConfigurations).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(GatewayConfigurations.VollScheduler),
                ValidateAttribute = _ => { }
            },
        });

        typeof(GatewayConfigurationsVollConfigurations).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "ClientName", Type = typeof(string) },
            new() { Name = "BaseUrl", Type = typeof(string) }
        });

        typeof(GatewayConfigurationsVollConfigurations).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(GatewayConfigurationsVollConfigurations.BaseUrl),
                ValidateAttribute = attr => { attr.AllowEmptyStrings.Should().BeFalse(); }
            },
        });
    }
}