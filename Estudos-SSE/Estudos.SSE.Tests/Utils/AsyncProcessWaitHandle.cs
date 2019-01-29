namespace Estudos.SSE.Tests.Utils
{
    public class AsyncProcessWaitHandle
    {
        private readonly ManualResetEventSlim _waitHandle = new(false);
        
        private static string _lastErrorMessage = "";

        public void Wait(TimeSpan timeout)
        {
            try
            {
                _waitHandle.Wait(timeout);

                if (!_waitHandle.Wait(0))
                {
                    throw new Exception(string.IsNullOrEmpty(_lastErrorMessage)
                                            ? "Async Process Not Completed!"
                                            : _lastErrorMessage);
                }
            }
            finally
            {
                _waitHandle.Reset();
            }
        }

        public void SetCompleted()
        {
            _waitHandle.Set();
            _lastErrorMessage = string.Empty;
        }
        
        public Task SetCompletedAsync()
        {
            _waitHandle.Set();
            _lastErrorMessage = string.Empty;
            return Task.CompletedTask;
        }
        
        public void SetFailed(string errorMessage)
        {
            _waitHandle.Set();
            _waitHandle.Reset();
            _lastErrorMessage = errorMessage;
        }
    }
}