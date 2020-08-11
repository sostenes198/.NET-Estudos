using Estudos.RemoteConfigurationProvider.Contracts;
using Microsoft.Extensions.Configuration;

namespace Estudos.RemoteConfigurationProvider.Configurations
{
    public class RemoteConfigurationSource : IConfigurationSource
    {
        public IRemoteFileInfo FileInfo { get; }


        public RemoteConfigurationSource(IRemoteFileInfo remoteFileInfo)
        {
            FileInfo = remoteFileInfo;
        }


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new RemoterConfigurationProvider(this);
        }
    }
}