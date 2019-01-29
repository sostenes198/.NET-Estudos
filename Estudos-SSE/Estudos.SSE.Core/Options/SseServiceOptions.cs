using System;
using Estudos.SSE.Core.Clients;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Options
{
    internal sealed class SseServiceOptions
    {
        public Action<IServiceProvider, SseClient>? OnClientConnected { get; set; }

        public Action<IServiceProvider, SseClient>? OnClientDisconnected { get; set; }
    }
}