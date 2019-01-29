using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Azure
{
    internal static class AzureSettings
    {
        public static IConfigurationRefresher AzureConfigurationRefresher { get; set; }
    }
}