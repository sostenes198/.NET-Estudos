using System;
using Estudos.SSE.Core.Clients;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Services.Events
{
    internal readonly struct SseClientConnectedArgs
    {
        public IServiceProvider ServiceProvider { get; }

        public SseClient Client { get; }

        public SseClientConnectedArgs(IServiceProvider serviceProvider, SseClient client)
        {
            ServiceProvider = serviceProvider;
            Client = client;
        }
    }
}