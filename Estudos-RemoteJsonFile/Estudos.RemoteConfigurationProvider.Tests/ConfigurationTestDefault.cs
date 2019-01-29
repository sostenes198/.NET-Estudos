using Estudos.RemoteConfigurationProvider.Configurations.RemoteFile;

namespace Estudos.RemoteConfigurationProvider.Tests
{
    public class ConfigurationTestDefault : ConfigurationTest
    {
        public ConfigurationTestDefault()
            : base(
                new RemoteFileInfoDefault("https://gitlab.com/sostenes198/remote-appssetings-public/-/raw/master/master.json", true, true), 
                new RemoteFileInfoDefault("https://gitlab.com/sostenes198/remote-appssetings-public/-/raw/master/master-notfound.json", true, true),
                new RemoteFileInfoDefault("https://gitlab.com/sostenes198/remote-appssetings-public/-/raw/master/master-notfound.json", false, true),
                "Public")
        {
        }
    }
}