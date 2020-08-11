using System.Collections.Generic;

namespace Estudos.RemoteConfigurationProvider.Contracts
{
    public interface IRemoteFileInfo
    {
        string Path { get; }
        IDictionary<string, string> ReadFile();
    }
}