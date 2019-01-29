using System.Threading.Tasks;
using Estudos.SSE.Core.Authorizations.Contracts;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Estudos.SSE.Core.Extensions;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedParameter.Local

namespace Estudos.SSE.Core.Middlewares
{
    internal sealed class SseMiddleware
    {
        private readonly ISseService _sseService;
        private readonly IAuthorizationSse _authorizationSse;
        private readonly IClientIdProvider _clientIdProvider;
        private readonly SseMiddlewareOptions _sseMiddlewareOptions;
        private readonly ILogger<SseMiddleware> _logger;

        public SseMiddleware(
            RequestDelegate _,
            ISseService sseService,
            IAuthorizationSse authorizationSse,
            IClientIdProvider clientIdProvider,
            IOptions<SseMiddlewareOptions> options,
            ILogger<SseMiddleware> logger
        )
        {
            _sseService = sseService;
            _authorizationSse = authorizationSse;
            _clientIdProvider = clientIdProvider;
            _sseMiddlewareOptions = options.Value;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!await _authorizationSse.AuthorizeAsync(context).ConfigureAwait(false))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                return;
            }

            var sseClientId = await _clientIdProvider.AcquireClientIdAsync(context).ConfigureAwait(false);

            if (await _sseService.IsClientConnectedAsync(sseClientId).ConfigureAwait(false))
            {
                _logger.LogWarning("The IServerSentEventsClient with identifier {ClientId} is already connected. The request can't have been accepted", sseClientId);
                context.Response.StatusCode = StatusCodes.Status409Conflict;

                return;
            }

            DisableResponseBuffering(context);
            HandleContentEncoding(context);

            await context.Response.AcceptAsync(_sseMiddlewareOptions.OnPrepareAccept).ConfigureAwait(false);

            var client = new SseClient(sseClientId, context.Response);
            await _sseService.ConnectClientAsync(client).ConfigureAwait(false);

            await context.RequestAborted.WaitAsync().ConfigureAwait(false);

            await _sseService.DisconnectClientAsync(client).ConfigureAwait(false);
            
            SetResultStatusCode(context);
        }

        private static void DisableResponseBuffering(HttpContext context)
        {
            var bufferingFeature = context.Features.Get<IHttpResponseBodyFeature>();

            bufferingFeature.DisableBuffering();

            context.Response.Headers.Append(SseConstants.DisabledBufferingHeader, SseConstants.DisabledBuffered);
        }

        private static void HandleContentEncoding(HttpContext context)
        {
            context.Response.OnStarting(ResponseOnStartingCallback, context);

            context.Response.Headers.Append(SseConstants.HandledContentResultHeader, SseConstants.HandledContentResult);
        }

        private static Task ResponseOnStartingCallback(object context)
        {
            var response = ((HttpContext) context).Response;

            if (!response.Headers.ContainsKey(SseConstants.ContentEncodingHeader))
            {
                response.Headers.Append(SseConstants.ContentEncodingHeader, SseConstants.IdentityContentEncoding);
            }

            response.Headers.Append(SseConstants.CacheControl, SseConstants.SseCacheControl);
            response.Headers.Append(SseConstants.Connection, SseConstants.SseConnection);

            return Task.CompletedTask;
        }

        private static void SetResultStatusCode(HttpContext context) => context.Response.StatusCode = StatusCodes.Status200OK;
    }
}