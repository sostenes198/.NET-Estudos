namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Configurations.AppConfigurations
{
    internal sealed class FilterConfiguration
    {
        public string KeyFilter { get; set; }
        public string LabelFilter { get; set; }

        public FilterConfiguration()
        {
            KeyFilter = Microsoft.Extensions.Configuration.AzureAppConfiguration.KeyFilter.Any;
            LabelFilter = Microsoft.Extensions.Configuration.AzureAppConfiguration.LabelFilter.Null;
        }
    }
}