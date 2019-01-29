using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Cronos;
using Microsoft.Extensions.Hosting;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob
{
    
    [ExcludeFromCodeCoverage]
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer _timer;
        private readonly CronExpression _expression;
        private readonly TimeZoneInfo _timeZoneInfo;

        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo)
        {
            _expression = CronExpression.Parse(cronExpression);
            _timeZoneInfo = timeZoneInfo;
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJobAsync(cancellationToken).ConfigureAwait(false);
        }

        private async Task ScheduleJobAsync(CancellationToken cancellationToken)
        {
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0) // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJobAsync(cancellationToken).ConfigureAwait(false);
                }

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (_, _) =>
                {
                    _timer.Dispose(); // reset and dispose timer
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWorkAsync(cancellationToken).ConfigureAwait(false);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJobAsync(cancellationToken).ConfigureAwait(false); // reschedule next
                    }
                };
                _timer.Start();
            }

            await Task.CompletedTask;
        }

        protected abstract Task DoWorkAsync(CancellationToken cancellationToken);

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}