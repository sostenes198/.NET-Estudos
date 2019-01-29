using Estudos.RemoteConfigurationProvider.Gitlab;

namespace Estudos.RemoteConfigurationProvider.Tests
{
    public class ConfigurationTestGitlab : ConfigurationTest
    {
        public ConfigurationTestGitlab() : base(
            new RemoteFileInfoGitlab("https://gitlab.com/api/v4/projects/20464339/repository/files/master.json?ref=master", "gzA_QkJTQ4eyAUHCUk1W", false, true),
            new RemoteFileInfoGitlab("https://gitlab.com/api/v4/projects/20464339/repository/files/masternotfound.json?ref=master", "gzA_QkJTQ4eyAUHCUk1W", true, true),
            new RemoteFileInfoGitlab("https://gitlab.com/api/v4/projects/20464339/repository/files/masternotfound.json?ref=master", "gzA_QkJTQ4eyAUHCUk1W", false, true),
            "Private")
        {
        }
    }
}