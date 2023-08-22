using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace Estudos.CleanArchitecture.Modular.IntegratedTests.Tests.API;

public class AppSettingsConfigurationTest : BaseTest
{
    [Fact(DisplayName = "Deve validar AppSettings.json da aplicação")]
    public void ShouldValidateAppSettingsJsonApplication()
    {
        // arrange
            var expectedConfigs = new SortedDictionary<string, string>
            {
                {"AllowedHosts", "*"},
                {"Serilog:Enrich:0", "FromLogContext"},
                {"Serilog:Enrich:1", "WithExceptionDetails"},
                {"Serilog:Enrich:2", "WithMachineName"},
                {"Serilog:Enrich:3", "WithProcessId"},
                {"Serilog:Enrich:4", "WithProcessName"},
                {"Serilog:Enrich:5", "WithCorrelationId"},
                {"Serilog:Enrich:6", "WithCorrelationIdHeader"},
                {"Serilog:MinimumLevel:Default", "Information"},
                {"Serilog:MinimumLevel:Override:Microsoft", "Warning"},
                {"Serilog:MinimumLevel:Override:Microsoft.AspNetCore", "Warning"},
                {"Serilog:MinimumLevel:Override:Confluent.Kafka", "6"},
                {"Serilog:Properties:Application", "Estudos.CleanArchitecture.Modular"},
                {"Serilog:Using:0", "Serilog.Sinks.Console"},
                {"Serilog:Using:1", "Serilog.Exceptions"},
                {"Serilog:Using:2", "Serilog.Expressions"},
                {"Serilog:Using:3", "Serilog.Sinks.Seq"},
                {"Serilog:WriteTo:0:Name", "Console"},
                {"Serilog:WriteTo:1:Args:serverUrl", "http://localhost:5341"},
                {"Serilog:WriteTo:1:Name", "Seq"}
            };
            var configs = GetConfigs();

            // act - assert
            configs.Should().BeEquivalentTo(expectedConfigs, "Alterações nas chaves LEMBRAR DE ATUALIZAR projeto de CONFIG");
    }

    private static SortedDictionary<string, string> GetConfigs()
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