using System;
using System.Diagnostics.CodeAnalysis;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob.Configs;
using Microsoft.Extensions.DependencyInjection;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob
{
    [ExcludeFromCodeCoverage]
    public static class ScheduledServiceExtensions
    {
        public static IServiceCollection AddHostedCronJobService<T>(this IServiceCollection services, Action<IScheduleConfig<T>> options) where T : CronJobService
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");
            }
            var config = new ScheduleConfig<T>();
            options.Invoke(config);
            if (string.IsNullOrWhiteSpace(config.CronExpression))
            {
                throw new ArgumentNullException(nameof(ScheduleConfig<T>.CronExpression), @"Empty Cron Expression is not allowed.");
            }

            services.AddSingleton<IScheduleConfig<T>>(config);
            services.AddHostedService<T>();
            return services;
        }
    }
}