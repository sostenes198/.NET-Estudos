using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;

namespace Scheduled.Message.Tests.Integration.Infrastructure.Databases.Configurations;

public class MongoConfigurationsTest : BaseTest
{
    [Fact]
    public void Should_Validate_Mongo_Configurations()
    {
        // Arrange 
        var expectedResult = new MongoConfigurations
        {
            Hangfire = new MongoConfigurationsHangfire
            {
                ConnectionString = "mongodb://localhost:27017/test?directConnection=true",
                HangfireParametersCollection = "hangfire.parameters"
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
        var result = ServiceProvider.GetRequiredService<IOptions<MongoConfigurations>>().Value;

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}