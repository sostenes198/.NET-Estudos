using System;
using System.Diagnostics.CodeAnalysis;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.FireForget;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Azure;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Contracts;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.CronJob;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ReactiveManagementConfigurationsExtensions
    {
        public static IHostBuilder ConfigureReactiveManagementConfiguration(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureReactiveAzureAppConfigurationProvider();
            return hostBuilder;
        }

        public static void AddReactiveManagementConfiguration(this IServiceCollection services)
        {
            services.TryAddSingleton<IAzureServiceBusTopicSubscription, AzureServiceBusTopicSubscription>();
            
            services.AddFireForgetHandler();
            
            services.AddHostedService<AzureServiceBusHostedService>();
           
            services.AddHostedCronJobService<ReactManagementPingTopicCronJobService>(c =>
            {
                c.TimeZoneInfo = TimeZoneInfo.Local;
                c.CronExpression = @"0 0 * * *";
            });

            services.AddAzureAppConfiguration();
        }

        public static void UserReactiveManagementConfiguration(this IApplicationBuilder app)
        {
            app.UseAzureAppConfiguration();
            app.UseReactiveManagementAppConfigurationReconnection();
        }
    }
}