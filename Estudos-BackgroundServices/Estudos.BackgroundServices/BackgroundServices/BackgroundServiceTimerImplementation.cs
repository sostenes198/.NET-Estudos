using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Estudos.BackgroundServices.BackgroundServices
{
    public class BackgroundServiceTimerImplementation : IHostedService
    {
        private CancellationTokenSource _stopping;
        private Timer? _timer;

        public BackgroundServiceTimerImplementation()
        {
            _stopping = new CancellationTokenSource();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(Timer_Tick, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(15));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _stopping.Cancel();
            }
            catch
            {
                // Ignore exceptions thrown as a result of a cancellation.
            }
            finally
            {
                _timer?.Dispose();
                _timer = null;
            }

            return Task.CompletedTask;
        }

        private async void Timer_Tick(object? state)
        {
            await RunAsync();
        }

        private async Task RunAsync()
        {
            try
            {
                await Task.Yield();
                var cancellation = CancellationTokenSource.CreateLinkedTokenSource(_stopping.Token);
                cancellation.CancelAfter(TimeSpan.FromSeconds(3));
                await Task.Delay(TimeSpan.FromSeconds(5), cancellation.Token);
            }
            catch
            {
                // ignored
            }
        }
    }
}