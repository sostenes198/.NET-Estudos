using System;
using Microsoft.AspNetCore.Http;
using Estudos.SSE.Core.Clients;
// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Extensions
{
    public class SseOptions
    {
        public int? CloseConnectionsInSecondsInterval { get; set; }

        public int? MaxTimeCacheInMinutes { get; set; }
        
        public Action<HttpResponse>? OnPrepareAccept { get; set; }
        
        public Action<IServiceProvider, SseClient>? OnClientConnected { get; set; }

        public Action<IServiceProvider, SseClient>? OnClientDisconnected { get; set; }
    }
}