using System;
using System.Threading;
using System.Threading.Tasks;

namespace Estudos.RemoteConfigurationProvider.Tests.Fixture
{
    public static class AsyncProcessWaitHandle
    {
        private static readonly ManualResetEventSlim WaitHandle = new ManualResetEventSlim(false);
        private static string _lastErrorMessage = "";

        public static void Wait(TimeSpan timeout)
        {
            try
            {
                WaitHandle.Wait(timeout);

                if (!WaitHandle.Wait(0))
                {
                    throw new Exception(string.IsNullOrEmpty(_lastErrorMessage)
                        ? "Async Process Not Completed!"
                        : _lastErrorMessage);
                }
            }
            finally
            {
                WaitHandle.Reset();
            }
        }

        public static void SetCompleted()
        {
            WaitHandle.Set();
            _lastErrorMessage = string.Empty;
        }
        
        public static Task SetCompletedAsync()
        {
            WaitHandle.Set();
            _lastErrorMessage = string.Empty;
            return Task.CompletedTask;
        }
        
        public static void SetFailed(string errorMessage)
        {
            WaitHandle.Set();
            WaitHandle.Reset();
            _lastErrorMessage = errorMessage;
        }
    }
}