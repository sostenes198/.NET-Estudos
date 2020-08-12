using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Estudos.RemoteConfigurationProvider.Configurations.RemoteFile;
using Estudos.RemoteConfigurationProvider.Gitlab.Dto;
using Estudos.RemoteConfigurationProvider.Helpers;
using Newtonsoft.Json;

namespace Estudos.RemoteConfigurationProvider.Gitlab
{
    public class RemoteFileInfoGitlab : RemoteFileInfo
    {
        public RemoteFileInfoGitlab(string path, string acessToken, bool optional = true, bool reload = false, int reloadTimeMinutes = 1) :
            base(path, optional, reload, reloadTimeMinutes, GitlabAuthenticationHttpHandler.Handler(acessToken))
        {
        }

        protected override IDictionary<string, string> ReadContent(HttpContent content)
        {
            var result = JsonConvert.DeserializeObject<ContentResultDto>(content.ReadAsStringAsync().GetAwaiter().GetResult());
            return ParseHelper.Parse(new MemoryStream(Convert.FromBase64String(result.Content)));
        }
    }
}