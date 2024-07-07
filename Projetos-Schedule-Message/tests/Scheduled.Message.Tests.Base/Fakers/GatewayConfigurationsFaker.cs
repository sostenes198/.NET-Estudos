using Scheduled.Message.Infrastructure.Gateways.Configurations;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class GatewayConfigurationsFaker
{
    public static GatewayConfigurations Create()
    {
        return new GatewayConfigurations
        {
            VollScheduler = new GatewayConfigurationsVollConfigurations
            {
                BaseUrl = "http://voll-scheduler-unit-test.com"
            }
        };
    }
}