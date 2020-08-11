using Estudos.RemoteConfigurationProvider.Configurations;
using Estudos.RemoteConfigurationProvider.Contracts;
using Microsoft.Extensions.Configuration;

namespace Estudos.RemoteConfigurationProvider
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder AddRemoteJsonFile(this IConfigurationBuilder builder, IRemoteFileInfo remoteFileInfo) =>
            builder.Add(new RemoteConfigurationSource(remoteFileInfo));
    }
}