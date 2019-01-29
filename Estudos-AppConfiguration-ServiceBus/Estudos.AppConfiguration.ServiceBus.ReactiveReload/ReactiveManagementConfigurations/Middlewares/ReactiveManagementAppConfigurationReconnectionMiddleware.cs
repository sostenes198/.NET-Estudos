using System.Threading.Tasks;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.FireForget;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Contracts;
using Microsoft.AspNetCore.Http;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Middlewares
{
    public class ReactiveManagementAppConfigurationReconnectionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IFireForgetHandler _fireForgetHandler;

        public ReactiveManagementAppConfigurationReconnectionMiddleware(RequestDelegate next, IFireForgetHandler fireForgetHandler)
        {
            _next = next;
            _fireForgetHandler = fireForgetHandler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _fireForgetHandler.ExecuteAsync<IAzureServiceBusTopicSubscription>(async serviceBusProcessorContext => { await serviceBusProcessorContext.TryReconnectAsync().ConfigureAwait(false); });
            await _next(context).ConfigureAwait(false);
        }
    }
}