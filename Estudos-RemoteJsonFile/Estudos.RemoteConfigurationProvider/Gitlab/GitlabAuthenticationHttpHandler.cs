using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.RemoteConfigurationProvider.Gitlab
{
    internal class GitlabAuthenticationHttpHandler : HttpClientHandler
    {
        private readonly string _acessToken;

        private GitlabAuthenticationHttpHandler(string acessToken)
        {
            _acessToken = acessToken;
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Private-Token", _acessToken);
            return base.SendAsync(request, cancellationToken);
        }
        
        public static GitlabAuthenticationHttpHandler Handler(string acessToken) =>
            new GitlabAuthenticationHttpHandler(acessToken);
    }
}