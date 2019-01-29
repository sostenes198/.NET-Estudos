using System;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob.Configs
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }
}