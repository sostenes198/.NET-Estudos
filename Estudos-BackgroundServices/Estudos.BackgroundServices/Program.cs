using Estudos.BackgroundServices.BackgroundServices;
using Estudos.BackgroundServices.BackgroundServices.BackgroundTasks;
using Estudos.BackgroundServices.BackgroundServices.ServiceWithScope;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Estudos.BackgroundServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // services.AddHostedService<TimedHostedService>();
                    
                    // services.AddHostedService<ConsumeScopedServiceHostedService>();
                    // services.AddScoped<IScopedProcessingService, ScopedProcessingService>();
                    
                    services.AddSingleton<MonitorLoop>();
                    services.AddHostedService<QueuedHostedService>();
                    services.AddSingleton<IBackgroundTaskQueue>(ctx => {
                        if (!int.TryParse(hostContext.Configuration["QueueCapacity"], out var queueCapacity))
                            queueCapacity = 100;
                        return new BackgroundTaskQueue(queueCapacity);
                    });
                });
    }
}