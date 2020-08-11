using Estudos.RemoteConfigurationProvider.Gitlab;

namespace Estudos.RemoteConfigurationProvider.Tests
{
    public class ConfigurationTestGitlab : ConfigurationTest
    {
        public ConfigurationTestGitlab() : base(
            new RemoteFileInfoGitlab("https://gitlab.com/api/v4/projects/20464339/repository/files/master%2Ejson?ref=master", "gzA_QkJTQ4eyAUHCUk1W"),
            new RemoteFileInfoGitlab("https://gitlab.com/api/v4/projects/20464339/repository/files/masternotfound%2Ejson?ref=master", "gzA_QkJTQ4eyAUHCUk1W"),
            new RemoteFileInfoGitlab("https://gitlab.com/api/v4/projects/20464339/repository/files/masternotfound%2Ejson?ref=master", "gzA_QkJTQ4eyAUHCUk1W", false),
            "Private")
        {
        }
    }
}