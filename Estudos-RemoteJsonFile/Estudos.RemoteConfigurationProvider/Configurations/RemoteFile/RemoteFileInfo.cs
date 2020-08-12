using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using Estudos.RemoteConfigurationProvider.Contracts;
using Timer = System.Timers.Timer;

namespace Estudos.RemoteConfigurationProvider.Configurations.RemoteFile
{
    public abstract class RemoteFileInfo : IRemoteFileInfo
    {
        public string Path { get; }
        public bool Reload { get; }
        public int ReloadTimeMinutes { get; }

        private readonly bool _optional;
        private readonly HttpClient _httpClient;
        private readonly int IntervalInMiliseconds = 60000;

        protected RemoteFileInfo(string path, bool optional = true, bool reload = false, int reloadTimeMinutes = 1,  HttpClientHandler httpClientHandler = default)
        {
            ValidateParams(path, reloadTimeMinutes);
            Path = path;
            Reload = reload;
            ReloadTimeMinutes = reloadTimeMinutes;
            _optional = optional;
            _httpClient = new HttpClient(httpClientHandler ?? new HttpClientHandler());
        }

        private void ValidateParams(string path, int reloadTimeMinutes)
        {
            if(string.IsNullOrWhiteSpace(path))
                throw new ArgumentException(nameof(Path));
            
            if(reloadTimeMinutes < 1)
                throw new ArgumentException(nameof(ReloadTimeMinutes));
        }
        
        public void Watch(Action toWatch)
        {
            var timer = new Timer {Interval = ReloadTimeMinutes * IntervalInMiliseconds, AutoReset = true, Enabled = true};
            timer.Elapsed += (source, e) => ThreadPool.QueueUserWorkItem(_ => toWatch?.Invoke());
        }

        public IDictionary<string, string> ReadFile() => TryGeResult(_httpClient.GetAsync(Path).GetAwaiter().GetResult());
       

        private IDictionary<string, string> TryGeResult(HttpResponseMessage responseMessage)
        {
            var resultStatusCode = responseMessage.IsSuccessStatusCode;
            if (_optional == false && resultStatusCode == false)
                throw new FileNotFoundException($"The configuration from remote file '{Path}' was not found and is not optional.");

            return resultStatusCode
                ? ReadContent(responseMessage.Content)
                : new Dictionary<string, string>();
        }

        protected abstract IDictionary<string, string> ReadContent(HttpContent content);
    }
}