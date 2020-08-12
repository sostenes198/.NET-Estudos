using Microsoft.Extensions.Configuration;

namespace Estudos.RemoteConfigurationProvider.Configurations
{
    public class RemoterConfigurationProvider : ConfigurationProvider
    {
        private readonly RemoteConfigurationSource _source;

        public RemoterConfigurationProvider(RemoteConfigurationSource source)
        {
            _source = source;
            
            if(source.FileInfo.Reload)
                _source.FileInfo.Watch(Load);
        }

        public override void Load()
        {
            Data = _source.FileInfo.ReadFile();
        }

    }
}