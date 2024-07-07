using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;
using Scheduled.Message.Tests.Base.Fakers;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Infrastructure.Databases.Mongo.Configurations;

public class MongoConfigurationsTest
{
    [Fact]
    public void Should_Validate_Section_Path()
    {
        // Arrange - Act - Assert
        MongoConfigurations.Section.Should().BeEquivalentTo("Mongo");
    }

    [Fact]
    public void Should_Validate_Mongo_Configurations()
    {
        // Arrange
        var expectedResult = new MongoConfigurations
        {
            Hangfire = new MongoConfigurationsHangfire
            {
                ConnectionString = "ConnectionString",
                HangfireParametersCollection = "HangfireParametersCollection"
            },
            CreateIndex = new MongoConfigurationsCreateIndex
            {
                Enabled = true,
                TTL = new MongoConfigurationsCreateIndexTTL
                {
                    Enabled = true
                }
            }
        };

        // Act
        var result = MongoConfigurationsFaker.Create();

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Should_Validate_Properties_Mongo_Configurations()
    {
        // Arrange - Act - Assert
        typeof(MongoConfigurations).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Section", Type = typeof(string) },
            new() { Name = "Hangfire", Type = typeof(MongoConfigurationsHangfire) },
            new() { Name = "CreateIndex", Type = typeof(MongoConfigurationsCreateIndex) },
        });

        typeof(MongoConfigurations).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(MongoConfigurations.Hangfire),
                ValidateAttribute = _ => { }
            },
            new()
            {
                Property = nameof(MongoConfigurations.CreateIndex),
                ValidateAttribute = _ => { }
            }
        });

        typeof(MongoConfigurationsHangfire).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "ConnectionString", Type = typeof(string) },
            new() { Name = "HangfireParametersCollection", Type = typeof(string) },
        });

        typeof(MongoConfigurationsHangfire).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(MongoConfigurationsHangfire.ConnectionString),
                ValidateAttribute = attr => { attr.AllowEmptyStrings.Should().BeFalse(); }
            },
            new()
            {
                Property = nameof(MongoConfigurationsHangfire.HangfireParametersCollection),
                ValidateAttribute = attr => { attr.AllowEmptyStrings.Should().BeFalse(); }
            }
        });

        typeof(MongoConfigurationsCreateIndex).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Enabled", Type = typeof(bool) },
            new() { Name = "TTL", Type = typeof(MongoConfigurationsCreateIndexTTL) },
        });

        typeof(MongoConfigurationsCreateIndex).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(MongoConfigurationsCreateIndex.Enabled),
                ValidateAttribute = _ => { }
            },
            new()
            {
                Property = nameof(MongoConfigurationsCreateIndex.TTL),
                ValidateAttribute = _ => { }
            }
        });

        typeof(MongoConfigurationsCreateIndexTTL).ValidateProperties(new List<AssertProperty>
        {
            new() { Name = "Enabled", Type = typeof(bool) },
        });

        typeof(MongoConfigurationsCreateIndexTTL).ValidateAttribute(new List<AttributeProperty<RequiredAttribute>>
        {
            new()
            {
                Property = nameof(MongoConfigurationsCreateIndexTTL.Enabled),
                ValidateAttribute = _ => { }
            }
        });
    }
}