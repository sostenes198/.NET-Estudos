using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Estudos.Exame.Capitulo1.GerenciaFluxoPrograma.Threads.Tasks
{
    public class AsyncProcessWaitHandleStudy
    {
        private static readonly BlockingCollection<object> _queue = new BlockingCollection<object>(1);
        private static readonly ManualResetEventSlim _waitHandle = new ManualResetEventSlim(false);
        private static string LastErrorMessage = "";

        public static void Wait(TimeSpan timeout, CancellationToken cancellationToken = default)
        {
            _queue.Add(default, cancellationToken);

            try
            {
                _waitHandle.Wait(timeout);

                if (!_waitHandle.Wait(0))
                {
                    throw new Exception(string.IsNullOrEmpty(LastErrorMessage)
                        ? "Async Process Not Completed!"
                        : LastErrorMessage);
                }
            }
            finally
            {
                _queue.Take(cancellationToken);
                _waitHandle.Reset();
            }
        }

        public static void SetCompleted()
        {
            _waitHandle.Set();
            LastErrorMessage = string.Empty;
        }

        public static void SetFailed(string errorMessage)
        {
            _waitHandle.Set();
            _waitHandle.Reset();
            LastErrorMessage = errorMessage;
        }
    }
}