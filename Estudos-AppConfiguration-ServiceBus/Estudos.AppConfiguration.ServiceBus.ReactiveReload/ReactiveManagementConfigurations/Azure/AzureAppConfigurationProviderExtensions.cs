using System;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Configurations.AppConfigurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Azure
{
    internal static class AzureAppConfigurationProviderExtensions
    {
        public static IHostBuilder ConfigureReactiveAzureAppConfigurationProvider(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureAppConfiguration((_, config) =>
            {
                var settings = config.Build();
                var reactManagementAppConfiguration = new ReactManagementAppConfiguration();
                settings.GetSection(ReactManagementAppConfiguration.SectionName).Bind(reactManagementAppConfiguration);
                reactManagementAppConfiguration.Validate();
                config.AddAzureAppConfiguration(options =>
                {
                    options.Connect(reactManagementAppConfiguration.ConnectionString)
                        .Select(reactManagementAppConfiguration.Filter.KeyFilter)
                        .Select(reactManagementAppConfiguration.Filter.KeyFilter, reactManagementAppConfiguration.Filter.LabelFilter);

                    options.ConfigureRefresh(refresh =>
                    {
                        refresh.Register(reactManagementAppConfiguration.SentinelKey, true);
                        refresh.SetCacheExpiration(TimeSpan.FromHours(reactManagementAppConfiguration.CacheExpirationInHours));
                    });
                    AzureSettings.AzureConfigurationRefresher = options.GetRefresher();
                }, true);
            });
            return hostBuilder;
        }
    }
}