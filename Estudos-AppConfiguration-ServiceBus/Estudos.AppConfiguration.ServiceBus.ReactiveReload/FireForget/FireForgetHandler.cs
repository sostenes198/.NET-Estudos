using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.FireForget
{
    internal sealed class FireForgetHandler : IFireForgetHandler
    {
        private readonly ILogger<FireForgetHandler> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public FireForgetHandler(ILogger<FireForgetHandler> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public void Execute<T>(Action<T> execution, Action<Exception> handlerException = null)
        {
            Task.Run(() =>
            {
                using var scope = CreateScope();
                var dependency = GetDependency<T>(scope);
                try
                {
                    execution(dependency);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Falha ao executar método fire-forget {message}", exception.Message);
                    handlerException?.Invoke(exception);
                }
            });
        }

        public void ExecuteAsync<T>(Func<T, Task> execution, Action<Exception> handlerException = null)
        {
            Task.Run(async () =>
            {
                using var scope = CreateScope();
                var dependency = GetDependency<T>(scope);
                try
                {
                    await execution(dependency).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Falha ao executar método fire-forget {message}", exception.Message);
                    handlerException?.Invoke(exception);
                }
            });
        }
        
        private IServiceScope CreateScope() =>_scopeFactory.CreateScope();
        
        private static T GetDependency<T>(IServiceScope scope) => scope.ServiceProvider.GetRequiredService<T>(); 
    }
}