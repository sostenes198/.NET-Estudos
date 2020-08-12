using System.Collections.Generic;
using System.Net.Http;
using Estudos.RemoteConfigurationProvider.Helpers;

namespace Estudos.RemoteConfigurationProvider.Configurations.RemoteFile
{
    public class RemoteFileInfoDefault : RemoteFileInfo
    {
        public RemoteFileInfoDefault(string path, bool optional = true, bool reload = false, int reloadTimeMinutes = 1, HttpClientHandler httpClientHandler = default)
            : base(path, optional, reload, reloadTimeMinutes, httpClientHandler)
        {
        }

        protected override IDictionary<string, string> ReadContent(HttpContent content) =>
            ParseHelper.Parse(content.ReadAsStreamAsync().GetAwaiter().GetResult());
    }
}