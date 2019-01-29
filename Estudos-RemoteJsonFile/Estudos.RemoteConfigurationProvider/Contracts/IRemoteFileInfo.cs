using System;
using System.Collections.Generic;

namespace Estudos.RemoteConfigurationProvider.Contracts
{
    public interface IRemoteFileInfo
    {
        string Path { get; }
        bool Reload { get; }
        int ReloadTimeMinutes { get; }
        IDictionary<string, string> ReadFile();
        void Watch(Action toWatch);
    }
}