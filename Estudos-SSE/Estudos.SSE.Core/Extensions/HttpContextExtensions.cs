using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
// ReSharper disable UnusedTupleComponentInReturnValue

namespace Estudos.SSE.Core.Extensions
{
    internal static class HttpContextExtensions
    {
        private const bool CompletedResult = true;
        
        public static Task AcceptAsync(this HttpResponse response, Action<HttpResponse>? onPrepareAccept = default)
        {
            response.ContentType = SseConstants.SseContentType;
            onPrepareAccept?.Invoke(response);

            return response.Body.FlushAsync();
        }

        public static Task WaitAsync(this CancellationToken cancellationToken)
        {
            var (cancellationTokenCompletionSource, _) = RegisterTokenAsync(cancellationToken);

            return cancellationToken.IsCancellationRequested ? Task.CompletedTask : cancellationTokenCompletionSource.Task;
        }

        private static (TaskCompletionSource<bool>, CancellationTokenRegistration) RegisterTokenAsync(CancellationToken cancellationToken)
        {
            TaskCompletionSource<bool> cancellationTokenCompletionSource = new TaskCompletionSource<bool>();
            var cancellationTokenRegistration = cancellationToken.Register(CancellationTokenCallback, cancellationTokenCompletionSource);

            return (cancellationTokenCompletionSource, cancellationTokenRegistration);
        }

        private static void CancellationTokenCallback(object? taskCompletionSource)
        {
            // Stryker disable once all
            ((TaskCompletionSource<bool>) taskCompletionSource!).SetResult(CompletedResult);
        }
    }
}