using System;
using System.Threading;
using System.Threading.Tasks;
using Estudos.SSE.Core.Events;
using Microsoft.AspNetCore.Http;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Clients
{
    public sealed class SseClient
    {
        private readonly HttpResponse _httpResponse;
        public string Id { get; }
        public bool IsConnected { get; private set; }

        public SseClient(in string id, in HttpResponse httpResponse)
        {
            Id = id;
            IsConnected = true;
            _httpResponse = httpResponse;
        }

        public bool Disconnect()
        {
            if (IsConnected)
            {
                IsConnected = false;
                _httpResponse.HttpContext.Abort();

                return true;
            }

            return false;
        }

        public Task SendEventAsync(SseEventBytes serverSentSseEvent, CancellationToken cancellationToken)
        {
            CheckIsConnected();
            return _httpResponse.Body.WriteAsync(serverSentSseEvent.Bytes, 0, serverSentSseEvent.BytesCount, cancellationToken);
        }

        private void CheckIsConnected()
        {
            if (!IsConnected)
                throw new InvalidOperationException("The client isn't connected.");
        }
    }
}