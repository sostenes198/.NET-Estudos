using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Scheduled.Message.Tests.Integration.Api;

public class AppSettingsConfigurationTest : BaseTest
{
    public AppSettingsConfigurationTest()
    {
        JsonFile = "appsettings.Example.json";
    }

    [Fact]
    public void Should_Validate_AppSettings_Json_Application()
    {
        // arrange
        var expectedConfigs = new SortedDictionary<string, string>
        {
            { "AllowedHosts", "*" },
            { "Gateways:VollScheduler:BaseUrl", "http://local-voll-scheduler-test.com" },
            { "Hangfire:TtlHangfireDocumentInDays", "60" },
            { "Logging:LogLevel:Default", "Information" },
            { "Logging:LogLevel:Hangfire", "Warning" },
            { "Logging:LogLevel:Microsoft.AspNetCore", "Warning" },
            { "Mongo:CreateIndex:Enabled", "True" },
            { "Mongo:CreateIndex:TTL:Enabled", "True" },
            { "Mongo:Hangfire:ConnectionString", "mongodb://localhost:27017/test?directConnection=true" },
            { "Mongo:Hangfire:HangfireParametersCollection", "hangfire.parameters" }
        };

        var configs = GetConfigs();

        // act - assert
        configs.Should()
            .BeEquivalentTo(expectedConfigs, "Changes to keys REMEMBER TO UPDATE CONFIG project");
    }

    private SortedDictionary<string, string> GetConfigs()
    {
        var configurationRoot = new ConfigurationBuilder()
            .AddJsonFile(JsonFile)
            .Build();

        var configs = new SortedDictionary<string, string>();

        var allChildrens = configurationRoot.GetChildren();

        foreach (var child in allChildrens)
        {
            if (child.Value != null)
            {
                configs.Add(child.Path, child.Value);
                continue;
            }

            RunInChildren(child);
        }

        void RunInChildren(IConfigurationSection section)
        {
            foreach (var child in section.GetChildren())
            {
                if (child.Value != null)
                {
                    configs.Add(child.Path, child.Value);
                    continue;
                }

                RunInChildren(child);
            }
        }

        return configs;
    }
}