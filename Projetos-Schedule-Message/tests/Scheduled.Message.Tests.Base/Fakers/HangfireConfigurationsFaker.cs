using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class HangfireConfigurationsFaker
{
    public static HangfireConfigurations Create()
    {
        return new HangfireConfigurations
        {
            TtlHangfireDocumentInDays = 10
        };
    }
}