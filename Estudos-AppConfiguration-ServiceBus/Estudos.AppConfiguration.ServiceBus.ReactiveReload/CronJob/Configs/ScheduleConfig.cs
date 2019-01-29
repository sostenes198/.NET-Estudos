using System;
using System.Diagnostics.CodeAnalysis;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob.Configs
{
    [ExcludeFromCodeCoverage]
    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}