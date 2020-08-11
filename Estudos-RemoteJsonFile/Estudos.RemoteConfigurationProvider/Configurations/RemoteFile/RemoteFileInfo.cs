using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Estudos.RemoteConfigurationProvider.Contracts;

namespace Estudos.RemoteConfigurationProvider.Configurations.RemoteFile
{
    public abstract class RemoteFileInfo : IRemoteFileInfo
    {
        public string Path { get; }
        
        private readonly bool _optional;
        private readonly HttpClient _httpClient;

        protected RemoteFileInfo(string path, bool optional = true, HttpClientHandler httpClientHandler = default)
        {
            Path = path;
            _optional = optional;
            _httpClient = new HttpClient(httpClientHandler ?? new HttpClientHandler());
        }
        
        public IDictionary<string, string> ReadFile() =>TryGeResult(_httpClient.GetAsync(Path).GetAwaiter().GetResult());

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