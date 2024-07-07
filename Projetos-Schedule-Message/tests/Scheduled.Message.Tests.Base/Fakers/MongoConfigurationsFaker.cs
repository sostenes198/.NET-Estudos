using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class MongoConfigurationsFaker
{
    public static MongoConfigurations Create()
    {
        return new MongoConfigurations
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
    }
}