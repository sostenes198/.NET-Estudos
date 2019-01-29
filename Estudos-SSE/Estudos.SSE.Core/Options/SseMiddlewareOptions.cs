using System;
using Microsoft.AspNetCore.Http;
// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Options
{
    internal sealed class SseMiddlewareOptions
    {
        public Action<HttpResponse>? OnPrepareAccept { get; set; }
    }
}